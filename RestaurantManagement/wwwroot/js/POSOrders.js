document.addEventListener("DOMContentLoaded", function () {

    // =====================================================
    // Elements
    // =====================================================

    const posOrders =
        document.getElementById("posOrders");

    const ordersUrl =
        posOrders?.dataset.ordersUrl;

    const updateStatusUrl =
        posOrders?.dataset.updateStatusUrl;

    const searchInput =
        document.getElementById("ordersSearch");

    const sortSelect =
        document.getElementById("ordersSort");

    const statusFilter =
        document.getElementById("statusFilter");

    const tableWrapper =
        document.getElementById("ordersTableWrapper");

    const totalCountElement =
        document.getElementById("ordersTotalCount");

    const antiForgeryToken =
        document.querySelector(
            'input[name="__RequestVerificationToken"]'
        )?.value ?? "";


    // =====================================================
    // Current Status Filter
    //
    // default = active orders only
    // all     = all orders
    // number  = specific status
    // =====================================================

    let currentStatus =
        "default";


    // =====================================================
    // Search Delay
    // =====================================================

    let searchTimeout = null;


    // =====================================================
    // Format UTC Dates
    // =====================================================

    function formatOrderDates() {

        document
            .querySelectorAll(".order-date")
            .forEach(element => {

                const utcValue =
                    element.dataset.utc;

                if (!utcValue) {
                    return;
                }

                const date =
                    new Date(utcValue);

                if (Number.isNaN(date.getTime())) {
                    return;
                }

                element.textContent =
                    new Intl.DateTimeFormat(
                        undefined,
                        {
                            day: "2-digit",
                            month: "2-digit",
                            year: "numeric",
                            hour: "2-digit",
                            minute: "2-digit"
                        }
                    ).format(date);

            });
    }


    // =====================================================
    // Update Total Count
    // =====================================================

    function updateTotalCount() {

        const rows =
            document.querySelectorAll(
                "#ordersTable tbody .order-row"
            );

        const count =
            rows.length;

        if (totalCountElement) {

            totalCountElement.textContent =
                count;

        }

    }


    // =====================================================
    // Set Active Status Button
    // =====================================================

    function setActiveStatusButton() {

        const buttons =
            statusFilter.querySelectorAll(
                ".status-filter-btn"
            );

        buttons.forEach(button => {

            button.classList.remove("active");

        });


        const activeButton =
            statusFilter.querySelector(
                `[data-status="${currentStatus}"]`
            );


        if (activeButton) {

            activeButton.classList.add(
                "active"
            );

        }

    }


    // =====================================================
    // Load Orders
    // =====================================================

    async function loadOrders() {

        if (!ordersUrl || !tableWrapper) {
            return;
        }


        const params =
            new URLSearchParams();


        // =================================================
        // Search
        // =================================================

        const search =
            searchInput?.value.trim() ?? "";


        if (search) {

            params.append(
                "Search",
                search
            );

        }


        // =================================================
        // Status
        // =================================================

        if (currentStatus === "all") {

            params.append(
                "ShowAllStatuses",
                "true"
            );

        }
        else if (
            currentStatus !== "default"
        ) {

            params.append(
                "Status",
                currentStatus
            );

        }


        // =================================================
        // Sort
        // =================================================

        if (sortSelect?.value) {

            params.append(
                "Sort",
                sortSelect.value
            );

        }


        try {

            tableWrapper.classList.add(
                "is-loading"
            );


            const url =
                `${ordersUrl}?${params.toString()}`;


            const response =
                await fetch(
                    url,
                    {
                        method: "GET",

                        headers: {

                            "X-Requested-With":
                                "XMLHttpRequest"

                        }
                    }
                );


            if (!response.ok) {

                throw new Error(
                    `Failed to load orders. (${response.status})`
                );

            }


            const html =
                await response.text();


            // Replace only PartialView

            tableWrapper.innerHTML =
                html;


            // Format new dates

            formatOrderDates();


            // Update count

            updateTotalCount();

        }
        catch (error) {

            console.error(error);

            alert(
                error.message ??
                "Could not load orders."
            );

        }
        finally {

            tableWrapper.classList.remove(
                "is-loading"
            );

        }

    }


    // =====================================================
    // STATUS FILTER
    // =====================================================

    if (statusFilter) {

        statusFilter.addEventListener(
            "click",
            function (event) {

                const button =
                    event.target.closest(
                        ".status-filter-btn"
                    );


                if (!button) {
                    return;
                }


                currentStatus =
                    button.dataset.status;


                setActiveStatusButton();

                loadOrders();

            }
        );

    }


    // =====================================================
    // SORT
    // =====================================================

    if (sortSelect) {

        sortSelect.addEventListener(
            "change",
            function () {

                loadOrders();

            }
        );

    }


    // =====================================================
    // SEARCH
    // =====================================================

    if (searchInput) {

        searchInput.addEventListener(
            "input",
            function () {

                clearTimeout(
                    searchTimeout
                );


                searchTimeout =
                    setTimeout(
                        function () {

                            loadOrders();

                        },
                        400
                    );

            }
        );

    }


    // =====================================================
    // UPDATE ORDER STATUS
    // Event Delegation
    // =====================================================

    document.addEventListener(
        "click",
        async function (event) {

            const button =
                event.target.closest(
                    ".status-option"
                );


            if (!button) {
                return;
            }


            const statusOptions =
                button.closest(
                    ".status-options"
                );


            const row =
                button.closest(
                    ".order-row"
                );


            if (!statusOptions || !row) {
                return;
            }


            const orderId =
                statusOptions.dataset.orderId;


            const newStatus =
                parseInt(
                    button.dataset.status
                );


            if (!orderId ||
                Number.isNaN(newStatus)) {

                return;
            }


            const buttons =
                statusOptions.querySelectorAll(
                    ".status-option"
                );


            buttons.forEach(b => {

                b.disabled = true;

            });


            try {

                const response =
                    await fetch(
                        updateStatusUrl,
                        {
                            method: "POST",

                            headers: {

                                "Content-Type":
                                    "application/json",

                                "RequestVerificationToken":
                                    antiForgeryToken

                            },

                            body: JSON.stringify({

                                orderId:
                                    orderId,

                                status:
                                    newStatus

                            })

                        }
                    );


                const responseText =
                    await response.text();


                if (!response.ok) {

                    let message =
                        `Request failed (${response.status})`;


                    if (responseText.trim()) {

                        try {

                            const errorResult =
                                JSON.parse(
                                    responseText
                                );

                            message =
                                errorResult.message ??
                                message;

                        }
                        catch {

                            message =
                                responseText;

                        }

                    }

                    throw new Error(
                        message
                    );

                }


                // =================================================
                // Completed / Cancelled
                // =================================================

                if (
                    newStatus === 4 ||
                    newStatus === 5
                ) {

                    row.style.opacity = "0";


                    setTimeout(
                        function () {

                            row.remove();

                            updateTotalCount();


                            const tbody =
                                document.querySelector(
                                    "#ordersTable tbody"
                                );


                            if (
                                tbody &&
                                tbody.children.length === 0
                            ) {

                                const table =
                                    document.getElementById(
                                        "ordersTable"
                                    );


                                if (table) {
                                    table.remove();
                                }


                                tableWrapper.innerHTML = `
                                    <div class="empty-orders">

                                        <div class="empty-icon">

                                            <svg viewBox="0 0 24 24"
                                                 fill="none"
                                                 stroke="currentColor"
                                                 stroke-width="2">

                                                <circle cx="12"
                                                        cy="12"
                                                        r="9" />

                                                <path d="M8 12h8" />

                                            </svg>

                                        </div>

                                        <h3>
                                            No orders found
                                        </h3>

                                        <p>
                                            Try changing your search or filters.
                                        </p>

                                    </div>
                                `;

                            }

                        },
                        250
                    );


                    return;
                }


                // =================================================
                // Update Row Color
                // =================================================

                row.classList.remove(
                    "status-confirmed",
                    "status-preparing",
                    "status-pending",
                    "status-ready",
                    "status-completed",
                    "status-cancelled"
                );


                const statusClasses = {

                    0: "status-confirmed",

                    1: "status-preparing",

                    2: "status-pending",

                    3: "status-ready",

                    4: "status-completed",

                    5: "status-cancelled"

                };


                const newClass =
                    statusClasses[newStatus];


                if (newClass) {

                    row.classList.add(
                        newClass
                    );

                }


                // =================================================
                // Active Status Button
                // =================================================

                buttons.forEach(b => {

                    b.classList.remove(
                        "active"
                    );

                });


                button.classList.add(
                    "active"
                );


                // =================================================
                // Success Effect
                // =================================================

                button.classList.add(
                    "status-updated"
                );


                setTimeout(
                    function () {

                        button.classList.remove(
                            "status-updated"
                        );

                    },
                    700
                );

            }
            catch (error) {

                console.error(error);

                alert(
                    error.message ??
                    "Could not update order status."
                );

            }
            finally {

                buttons.forEach(b => {

                    b.disabled = false;

                });

            }

        }
    );


    // =====================================================
    // Initial Date Formatting
    // =====================================================

    formatOrderDates();

});