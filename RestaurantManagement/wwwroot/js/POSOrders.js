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
    // =====================================================

    let currentStatus =
        "default";


    // =====================================================
    // Search Delay
    // =====================================================

    let searchTimeout = null;


    // =====================================================
    // Status Popup
    // =====================================================

    let statusPopup = null;

    let currentStatusButton = null;


    // =====================================================
    // Order Statuses
    // =====================================================

    const orderStatuses = [

        {
            value: 0,
            name: "Pending",
            className: "status-pending"
        },

        {
            value: 1,
            name: "Confirmed",
            className: "status-confirmed"
        },

        {
            value: 2,
            name: "Preparing",
            className: "status-preparing"
        },

        {
            value: 3,
            name: "Ready",
            className: "status-ready"
        },

        {
            value: 4,
            name: "Completed",
            className: "status-completed"
        },

        {
            value: 5,
            name: "Cancelled",
            className: "status-cancelled"
        }

    ];


    // =====================================================
    // Status Classes
    // =====================================================

    const statusClasses = {

        0: "status-pending",
        1: "status-confirmed",
        2: "status-preparing",
        3: "status-ready",
        4: "status-completed",
        5: "status-cancelled"

    };


    // =====================================================
    // Create Status Popup
    // =====================================================

    function createStatusPopup() {

        if (statusPopup) {
            return statusPopup;
        }

        statusPopup =
            document.createElement("div");

        statusPopup.id =
            "statusPopup";

        statusPopup.className =
            "status-popup";

        document.body.appendChild(
            statusPopup
        );

        return statusPopup;
    }


    // =====================================================
    // Close Status Popup
    // =====================================================

    function closeStatusPopup() {

        if (!statusPopup) {
            return;
        }


        statusPopup.classList.remove(
            "show"
        );


        if (currentStatusButton) {

            currentStatusButton.setAttribute(
                "aria-expanded",
                "false"
            );

            currentStatusButton.classList.remove(
                "status-popup-open"
            );

        }


        currentStatusButton = null;
    }


    // =====================================================
    // Open Status Popup
    // =====================================================

    function openStatusPopup(button) {

        const popup =
            createStatusPopup();


        const orderCurrentStatus =
            parseInt(
                button.dataset.status
            );


        if (
            Number.isNaN(
                orderCurrentStatus
            )
        ) {
            return;
        }


        currentStatusButton =
            button;


        button.classList.add(
            "status-popup-open"
        );


        popup.innerHTML = "";


        // =================================================
        // Create ALL Status Options
        //
        // Current status stays visible and selected
        // =================================================

        orderStatuses.forEach(
            function (status) {

                const option =
                    document.createElement("button");


                option.type =
                    "button";


                option.className =
                    `status-option ${status.className}`;


                option.dataset.status =
                    status.value;


                option.textContent =
                    status.name;


                // =================================================
                // Current Status = Selected
                // =================================================

                if (
                    status.value ===
                    orderCurrentStatus
                ) {

                    option.classList.add(
                        "selected"
                    );


                    // Prevent unnecessary update
                    option.disabled =
                        true;

                }


                popup.appendChild(
                    option
                );

            }
        );


        // =================================================
        // Position Popup
        // =================================================

        const rect =
            button.getBoundingClientRect();


        popup.style.minWidth =
            `${Math.max(rect.width, 125)}px`;


        let top =
            rect.bottom + 6;


        let left =
            rect.left +
            (rect.width / 2) -
            (popup.offsetWidth / 2);


        // =================================================
        // Prevent Right Overflow
        // =================================================

        if (
            left + popup.offsetWidth >
            window.innerWidth - 10
        ) {

            left =
                window.innerWidth -
                popup.offsetWidth -
                10;

        }


        // =================================================
        // Prevent Left Overflow
        // =================================================

        if (
            left < 10
        ) {

            left = 10;

        }


        // =================================================
        // Prevent Bottom Overflow
        // =================================================

        if (
            top + popup.offsetHeight >
            window.innerHeight - 10
        ) {

            top =
                rect.top -
                popup.offsetHeight -
                6;

        }


        popup.style.top =
            `${top}px`;

        popup.style.left =
            `${left}px`;


        popup.classList.add(
            "show"
        );


        button.setAttribute(
            "aria-expanded",
            "true"
        );

    }


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


                if (
                    Number.isNaN(
                        date.getTime()
                    )
                ) {
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
    // Remove Order Row
    // =====================================================

    function removeOrderRow(row) {

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


                    if (tableWrapper) {

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

                }

            },
            150
        );

    }


    // =====================================================
    // Update Order Status
    // =====================================================

    async function updateOrderStatus(
        button,
        newStatus,
        sourceButton = null
    ) {

        const orderId =
            button.dataset.orderId;


        if (
            !orderId ||
            Number.isNaN(newStatus)
        ) {
            return;
        }


        const row =
            button.closest(
                ".order-row"
            );


        if (!row) {
            return;
        }


        if (sourceButton) {

            sourceButton.disabled =
                true;

        }


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


                if (
                    responseText.trim()
                ) {

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
            // Filters
            // =================================================

            const isAllFilter =
                currentStatus === "all";


            const isDefaultFilter =
                currentStatus === "default";


            const isSpecificStatusFilter =
                !isAllFilter &&
                !isDefaultFilter;


            // =================================================
            // SPECIFIC STATUS FILTER
            // =================================================

            if (
                isSpecificStatusFilter
            ) {

                const selectedFilterStatus =
                    parseInt(
                        currentStatus
                    );


                if (
                    selectedFilterStatus !==
                    newStatus
                ) {

                    removeOrderRow(
                        row
                    );


                    closeStatusPopup();

                    return;

                }

            }


            // =================================================
            // DEFAULT FILTER
            //
            // Completed / Cancelled disappear
            // =================================================

            if (
                isDefaultFilter &&
                (
                    newStatus === 4 ||
                    newStatus === 5
                )
            ) {

                removeOrderRow(
                    row
                );


                closeStatusPopup();

                return;

            }


            // =================================================
            // Update Row Status Class
            // =================================================

            row.classList.remove(
                "status-pending",
                "status-confirmed",
                "status-preparing",
                "status-ready",
                "status-completed",
                "status-cancelled"
            );


            const newClass =
                statusClasses[newStatus];


            if (newClass) {

                row.classList.add(
                    newClass
                );

            }


            // =================================================
            // Update Current Status Button
            // =================================================

            const selectedStatus =
                orderStatuses.find(
                    status =>
                        status.value ===
                        newStatus
                );


            if (selectedStatus) {

                button.classList.remove(
                    "status-pending",
                    "status-confirmed",
                    "status-preparing",
                    "status-ready",
                    "status-completed",
                    "status-cancelled"
                );


                button.classList.add(
                    selectedStatus.className
                );


                button.dataset.status =
                    newStatus;


                const text =
                    button.querySelector(
                        "span:first-child"
                    );


                if (text) {

                    text.textContent =
                        selectedStatus.name;

                }


         // =================================================
// UPDATE / REMOVE / CREATE NEXT BUTTON
// =================================================

let nextButton =
    row.querySelector(
        ".next-status-button"
    );


// =================================================
// Completed / Cancelled
// No Next button
// =================================================

if (newStatus >= 4) {

    if (nextButton) {

        nextButton.remove();

    }

}


// =================================================
// Pending / Confirmed / Preparing / Ready
// Next button must exist
// =================================================

else {

    if (!nextButton) {

        nextButton =
            document.createElement("button");


        nextButton.type =
            "button";


        nextButton.className =
            "next-status-button";


        nextButton.dataset.orderId =
            orderId;


        nextButton.dataset.status =
            newStatus;


        nextButton.title =
            "Move to next status";


        nextButton.setAttribute(
            "aria-label",
            "Move order to next status"
        );


        const arrow =
            document.createElement("span");


        arrow.className =
            "next-arrow";


        nextButton.appendChild(
            arrow
        );


        // Add it directly after Current Status button
        button.insertAdjacentElement(
            "afterend",
            nextButton
        );

    }
    else {

        nextButton.dataset.status =
            newStatus;

    }

}

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


            closeStatusPopup();

        }
        catch (error) {

            console.error(error);


            alert(
                error.message ??
                "Could not update order status."
            );

        }
        finally {

            if (sourceButton) {

                sourceButton.disabled =
                    false;

            }

        }

    }


    // =====================================================
    // STATUS BUTTON + POPUP + NEXT
    // =====================================================

    document.addEventListener(
        "click",
        async function (event) {

            // =================================================
            // NEXT STATUS
            // =================================================

            const nextButton =
                event.target.closest(
                    ".next-status-button"
                );


            if (nextButton) {

                event.stopPropagation();


                const row =
                    nextButton.closest(
                        ".order-row"
                    );


                if (!row) {
                    return;
                }


                const currentButton =
                    row.querySelector(
                        ".current-status-button"
                    );


                if (!currentButton) {
                    return;
                }


                const currentStatusValue =
                    parseInt(
                        currentButton.dataset.status
                    );


                if (
                    Number.isNaN(
                        currentStatusValue
                    )
                ) {
                    return;
                }


                // =================================================
                // Completed = final
                // Cancelled = final
                // =================================================

                if (
                    currentStatusValue >= 4
                ) {
                    return;
                }


                const nextStatus =
                    currentStatusValue + 1;


                await updateOrderStatus(
                    currentButton,
                    nextStatus,
                    nextButton
                );


                return;
            }


            // =================================================
            // OPEN / CLOSE STATUS POPUP
            // =================================================

            const currentButton =
                event.target.closest(
                    ".current-status-button"
                );


            if (currentButton) {

                if (
                    currentStatusButton ===
                    currentButton
                ) {

                    closeStatusPopup();

                    return;
                }


                closeStatusPopup();


                openStatusPopup(
                    currentButton
                );


                return;
            }


            // =================================================
            // STATUS OPTION
            // =================================================

            const option =
                event.target.closest(
                    ".status-popup .status-option"
                );


            if (!option) {

                if (
                    statusPopup &&
                    !event.target.closest(
                        ".status-popup"
                    )
                ) {

                    closeStatusPopup();

                }

                return;
            }


            // Current selected status is disabled
            if (option.disabled) {
                return;
            }


            const button =
                currentStatusButton;


            if (!button) {
                return;
            }


            const newStatus =
                parseInt(
                    option.dataset.status
                );


            if (
                Number.isNaN(
                    newStatus
                )
            ) {
                return;
            }


            option.disabled =
                true;


            await updateOrderStatus(
                button,
                newStatus,
                option
            );

        }
    );


    // =====================================================
    // STATUS FILTER
    // =====================================================

    if (statusFilter) {

        const selectButton =
            statusFilter.querySelector(
                ".status-select-button"
            );

        const selectMenu =
            statusFilter.querySelector(
                ".status-select-menu"
            );

        const selectedText =
            statusFilter.querySelector(
                "#selectedStatusText"
            );


        // =================================================
        // Open / Close
        // =================================================

        selectButton.addEventListener(
            "click",
            function (event) {

                event.stopPropagation();


                document
                    .getElementById("sortFilter")
                    ?.classList.remove("open");


                statusFilter.classList.toggle(
                    "open"
                );

            }
        );


        // =================================================
        // Select Status
        // =================================================

        selectMenu.addEventListener(
            "click",
            function (event) {

                const option =
                    event.target.closest(
                        ".status-option"
                    );


                if (!option) {
                    return;
                }


                currentStatus =
                    option.dataset.status;


                // Remove selected
                selectMenu
                    .querySelectorAll(".status-option")
                    .forEach(item => {

                        item.classList.remove(
                            "selected"
                        );

                    });


                // Add selected
                option.classList.add(
                    "selected"
                );


                // Update text
                selectedText.textContent =
                    option.textContent.trim();


                // Update color
                const optionClass =
                    [...option.classList]
                        .find(className =>
                            className.startsWith("status-") &&
                            className !== "status-option"
                        );


                const dot =
                    selectButton.querySelector(
                        ".status-dot"
                    );


                if (optionClass) {

                    dot.className =
                        `status-dot ${optionClass}`;

                }


                statusFilter.classList.remove(
                    "open"
                );


                loadOrders();

            }
        );


        // =================================================
        // Click Outside
        // =================================================

        document.addEventListener(
            "click",
            function (event) {

                if (
                    !statusFilter.contains(
                        event.target
                    )
                ) {

                    statusFilter.classList.remove(
                        "open"
                    );

                }

            }
        );

    }


    // =====================================================
    // SORT
    // =====================================================

    const sortDropdown =
        document.getElementById("sortFilter");


    if (sortDropdown) {

        const button =
            sortDropdown.querySelector(
                ".sort-select-button"
            );

        const input =
            document.getElementById("ordersSort");

        const text =
            document.getElementById("selectedSortText");

        const options =
            sortDropdown.querySelectorAll(
                ".sort-option"
            );


        // =================================================
        // Open / Close
        // =================================================

        button.addEventListener(
            "click",
            function (e) {

                e.stopPropagation();


                document
                    .getElementById("statusFilter")
                    ?.classList.remove("open");


                sortDropdown.classList.toggle(
                    "open"
                );

            }
        );


        // =================================================
        // Select Sort
        // =================================================

        options.forEach(
            function (option) {

                option.addEventListener(
                    "click",
                    function () {

                        options.forEach(
                            function (o) {

                                o.classList.remove(
                                    "selected"
                                );

                            }
                        );


                        option.classList.add(
                            "selected"
                        );


                        text.textContent =
                            option.textContent.trim();


                        input.value =
                            option.dataset.sort;


                        sortDropdown.classList.remove(
                            "open"
                        );


                        input.dispatchEvent(
                            new Event(
                                "change",
                                {
                                    bubbles: true
                                }
                            )
                        );

                    }
                );

            }
        );


        // =================================================
        // Click Outside
        // =================================================

        document.addEventListener(
            "click",
            function (e) {

                if (
                    !sortDropdown.contains(
                        e.target
                    )
                ) {

                    sortDropdown.classList.remove(
                        "open"
                    );

                }

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
    // Load Orders
    // =====================================================

    async function loadOrders() {

        if (
            !ordersUrl ||
            !tableWrapper
        ) {
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

        if (
            currentStatus === "all"
        ) {

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

        if (
            sortSelect?.value
        ) {

            params.append(
                "Sort",
                sortSelect.value
            );

        }


        try {

            tableWrapper.classList.add(
                "is-loading"
            );


            closeStatusPopup();


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


            // =================================================
            // Replace Partial
            // =================================================

            tableWrapper.innerHTML =
                html;


            // =================================================
            // Format Dates
            // =================================================

            formatOrderDates();


            // =================================================
            // Update Count
            // =================================================

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
    // Close Popup on Scroll
    // =====================================================

    window.addEventListener(
        "scroll",
        function () {

            closeStatusPopup();

        },
        true
    );


    // =====================================================
    // Close Popup on Resize
    // =====================================================

    window.addEventListener(
        "resize",
        function () {

            closeStatusPopup();

        }
    );


    // =====================================================
    // Initial Date Formatting
    // =====================================================

    formatOrderDates();

});