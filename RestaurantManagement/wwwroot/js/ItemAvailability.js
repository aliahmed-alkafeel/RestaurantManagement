document.addEventListener("DOMContentLoaded", () => {

    // =========================================================
    // ELEMENTS
    // =========================================================

    const availableContainer =
        document.getElementById("availableContainer");

    const unavailableContainer =
        document.getElementById("unavailableContainer");


    const availableSearch =
        document.getElementById("availableSearch");

    const unavailableSearch =
        document.getElementById("unavailableSearch");


    const availableCount =
        document.getElementById("availableCount");

    const unavailableCount =
        document.getElementById("unavailableCount");


    const availableEmpty =
        document.getElementById("availableEmpty");

    const unavailableEmpty =
        document.getElementById("unavailableEmpty");


    const availableNoResults =
        document.getElementById("availableNoResults");

    const unavailableNoResults =
        document.getElementById("unavailableNoResults");


    const tokenElement =
        document.querySelector(
            "#availabilityTokenForm input[name='__RequestVerificationToken']"
        );


    if (!tokenElement) {

        console.error(
            "Anti-forgery token was not found."
        );

        return;
    }


    const token =
        tokenElement.value;


    // =========================================================
    // CLICK ITEM
    // =========================================================

    document.addEventListener(
        "click",
        async (event) => {

            const card =
                event.target.closest(".item-card");


            if (!card)
                return;

            await toggleAvailability(card);
        }
    );


    // =========================================================
    // TOGGLE AVAILABILITY
    // =========================================================

    async function toggleAvailability(card) {

        if (
            card.classList.contains("moving")
        ) {
            return;
        }


        const itemId =
            card.dataset.itemId;


        card.classList.add("moving");


        try {

            const body =
                new URLSearchParams();


            body.append(
                "id",
                itemId
            );


            body.append(
                "__RequestVerificationToken",
                token
            );


            const response =
                await fetch(
                    "/POS/ToggleAvailability",
                    {
                        method: "POST",

                        headers: {
                            "Content-Type":
                                "application/x-www-form-urlencoded; charset=UTF-8"
                        },

                        body: body.toString()
                    }
                );


            const responseText =
                await response.text();


            if (!response.ok) {

                let message =
                    `Request failed (${response.status})`;


                try {

                    const error =
                        JSON.parse(responseText);


                    if (error.message) {
                        message =
                            error.message;
                    }

                }
                catch {
                    // Response was not JSON.
                }


                throw new Error(message);
            }


            if (!responseText) {

                throw new Error(
                    "Server returned an empty response."
                );
            }


            const result =
                JSON.parse(responseText);


            if (!result.success) {

                throw new Error(
                    result.message ||
                    "Unable to update item."
                );
            }


            // =================================================
            // MOVE
            // =================================================

            if (result.isAvailable) {

                moveToAvailable(
                    card,
                    result
                );

            }
            else {

                moveToUnavailable(
                    card,
                    result
                );
            }


            // =================================================
            // UPDATE UI
            // =================================================

            removeEmptyGroups();

            updateCounts();

            updateAllCategoryCounts();

            sortAvailableGroups();

            sortUnavailableGroups();

            refreshSearch();

        }
        catch (error) {

            console.error(
                "Availability update failed:",
                error
            );


            alert(
                error.message ||
                "Unable to update item availability."
            );


            card.classList.remove(
                "moving"
            );
        }
    }


    // =========================================================
    // MOVE TO AVAILABLE
    // =========================================================

    function moveToAvailable(
        card,
        item
    ) {

        updateCard(
            card,
            item
        );


        card.classList.remove(
            "unavailable-card"
        );


        const dot =
            card.querySelector(
                ".availability-dot"
            );


        dot.classList.remove(
            "unavailable-dot"
        );


        dot.classList.add(
            "available-dot"
        );


        let typeGroup =
            findTypeGroup(
                availableContainer,
                item.type
            );


        if (!typeGroup) {

            typeGroup =
                createTypeGroup(
                    item
                );


            availableContainer.appendChild(
                typeGroup
            );
        }


        let categoryGroup =
            findCategoryGroup(
                typeGroup,
                item.categoryId
            );


        if (!categoryGroup) {

            categoryGroup =
                createCategoryGroup(
                    item
                );


            typeGroup
                .querySelector(
                    ".categories-container"
                )
                .appendChild(
                    categoryGroup
                );
        }


        const grid =
            categoryGroup.querySelector(
                ".items-grid"
            );


        grid.appendChild(
            card
        );


        card.classList.remove(
            "moving"
        );
    }


    // =========================================================
    // MOVE TO UNAVAILABLE
    // =========================================================

    function moveToUnavailable(
        card,
        item
    ) {

        updateCard(
            card,
            item
        );


        card.classList.add(
            "unavailable-card"
        );


        const dot =
            card.querySelector(
                ".availability-dot"
            );


        dot.classList.remove(
            "available-dot"
        );


        dot.classList.add(
            "unavailable-dot"
        );


        let typeGroup =
            findTypeGroup(
                unavailableContainer,
                item.type
            );


        if (!typeGroup) {

            typeGroup =
                createTypeGroup(
                    item
                );


            unavailableContainer.appendChild(
                typeGroup
            );
        }


        let categoryGroup =
            findCategoryGroup(
                typeGroup,
                item.categoryId
            );


        if (!categoryGroup) {

            categoryGroup =
                createCategoryGroup(
                    item
                );


            typeGroup
                .querySelector(
                    ".categories-container"
                )
                .appendChild(
                    categoryGroup
                );
        }


        const grid =
            categoryGroup.querySelector(
                ".items-grid"
            );


        grid.appendChild(
            card
        );


        card.classList.remove(
            "moving"
        );
    }


    // =========================================================
    // UPDATE CARD
    // =========================================================

    function updateCard(
        card,
        item
    ) {

        card.dataset.itemId =
            item.id;

        card.dataset.itemName =
            item.itemName;

        card.dataset.categoryId =
            item.categoryId;

        card.dataset.categoryName =
            item.categoryName;

        card.dataset.type =
            item.type;

        card.dataset.typeName =
            item.typeName;

        card.dataset.price =
            item.price;

        card.dataset.image =
            item.imageUrl;


        card.dataset.search =
            `${item.itemName} ${item.categoryName} ${item.typeName}`
                .toLowerCase();


        const image =
            card.querySelector(
                ".item-image"
            );


        if (image) {

            image.src =
                item.imageUrl;

            image.alt =
                item.itemName;
        }


        const name =
            card.querySelector(
                ".item-name"
            );


        if (name) {

            name.textContent =
                item.itemName;
        }


        const category =
            card.querySelector(
                ".item-category"
            );


        if (category) {

            category.textContent =
                item.categoryName;
        }


        const price =
            card.querySelector(
                ".item-price"
            );


        if (price) {

            price.textContent =
                Number(item.price)
                    .toFixed(2);
        }
    }


    // =========================================================
    // FIND TYPE
    // =========================================================

    function findTypeGroup(
        container,
        type
    ) {

        const typeGroups =
            [...container.children]
                .filter(
                    element =>
                        element.classList.contains(
                            "type-group"
                        )
                );


        return typeGroups.find(
            group =>
                String(group.dataset.type) ===
                String(type)
        ) || null;

    }


    // =========================================================
    // FIND CATEGORY
    // =========================================================

    function findCategoryGroup(
        typeGroup,
        categoryId
    ) {

        const categoriesContainer =
            typeGroup.querySelector(
                ":scope > .categories-container"
            );


        if (!categoriesContainer)
            return null;


        const categories =
            [...categoriesContainer.children]
                .filter(
                    element =>
                        element.classList.contains(
                            "category-group"
                        )
                );


        return categories.find(
            category =>
                String(category.dataset.categoryId) ===
                String(categoryId)
        ) || null;
    }


    // =========================================================
    // CREATE TYPE
    // =========================================================

    function createTypeGroup(
        item
    ) {

        const typeGroup =
            document.createElement("div");


        typeGroup.className =
            "type-group";


        typeGroup.dataset.type =
            item.type;


        typeGroup.dataset.typeName =
            item.typeName;


        const header =
            document.createElement("div");


        header.className =
            "type-header";


        const line =
            document.createElement("span");


        line.className =
            "type-line";


        const title =
            document.createElement("h2");


        title.textContent =
            item.typeName;


        header.appendChild(line);

        header.appendChild(title);


        const categoriesContainer =
            document.createElement("div");


        categoriesContainer.className =
            "categories-container";


        typeGroup.appendChild(header);

        typeGroup.appendChild(
            categoriesContainer
        );


        return typeGroup;
    }


    // =========================================================
    // CREATE CATEGORY
    // =========================================================

    function createCategoryGroup(
        item
    ) {

        const categoryGroup =
            document.createElement("div");


        categoryGroup.className =
            "category-group";


        categoryGroup.dataset.categoryId =
            item.categoryId;


        categoryGroup.dataset.categoryName =
            item.categoryName;


        const header =
            document.createElement("div");


        header.className =
            "category-header";


        const categoryName =
            document.createElement("span");


        categoryName.textContent =
            item.categoryName;


        const categoryCount =
            document.createElement("span");


        categoryCount.className =
            "category-count";


        categoryCount.textContent =
            "0";


        header.appendChild(
            categoryName
        );


        header.appendChild(
            categoryCount
        );


        const grid =
            document.createElement("div");


        grid.className =
            "items-grid";


        categoryGroup.appendChild(
            header
        );


        categoryGroup.appendChild(
            grid
        );


        return categoryGroup;
    }


    // =========================================================
    // REMOVE EMPTY GROUPS
    // =========================================================

    function removeEmptyGroups() {

        const containers = [
            availableContainer,
            unavailableContainer
        ];


        containers.forEach(container => {

            // -----------------------------------------------
            // Categories
            // -----------------------------------------------

            const categories =
                container.querySelectorAll(
                    ".category-group"
                );


            categories.forEach(category => {

                const itemCount =
                    category.querySelectorAll(
                        ":scope > .items-grid > .item-card"
                    ).length;


                if (itemCount === 0) {

                    category.remove();
                }
            });


            // -----------------------------------------------
            // Types
            // -----------------------------------------------

            const types =
                container.querySelectorAll(
                    ":scope > .type-group"
                );


            types.forEach(type => {

                const itemCount =
                    type.querySelectorAll(
                        ".item-card"
                    ).length;


                if (itemCount === 0) {

                    type.remove();
                }
            });
        });
    }


    // =========================================================
    // UPDATE MAIN COUNTS
    // =========================================================

    function updateCounts() {

        const availableItems =
            availableContainer.querySelectorAll(
                ".item-card"
            );


        const unavailableItems =
            unavailableContainer.querySelectorAll(
                ".item-card"
            );


        availableCount.textContent =
            availableItems.length;


        unavailableCount.textContent =
            unavailableItems.length;
    }


    // =========================================================
    // UPDATE CATEGORY COUNTS
    // =========================================================

    function updateAllCategoryCounts() {

        document
            .querySelectorAll(
                ".category-group"
            )
            .forEach(category => {

                const count =
                    category.querySelectorAll(
                        ":scope > .items-grid > .item-card"
                    ).length;


                const counter =
                    category.querySelector(
                        ".category-count"
                    );


                if (counter) {

                    counter.textContent =
                        count;
                }
            });
    }


    // =========================================================
    // SEARCH
    // =========================================================

    availableSearch.addEventListener(
        "input",
        () => {

            console.log("SEARCH:", availableSearch.value);

            [...availableContainer.querySelectorAll(".item-card")]
                .forEach(card => {

                    console.log({
                        name: card.dataset.itemName,
                        search: card.dataset.search
                    });

                });
            filterContainer(
                availableContainer,
                availableSearch.value,
                availableEmpty,
                availableNoResults
            );
        }
    );


    unavailableSearch.addEventListener(
        "input",
        () => {

            filterContainer(
                unavailableContainer,
                unavailableSearch.value,
                unavailableEmpty,
                unavailableNoResults
            );
        }
    );


    // =========================================================
    // FILTER CONTAINER
    // =========================================================

    function filterContainer(
        container,
        searchValue,
        emptyElement,
        noResultsElement
    ) {

        const query =
            searchValue.trim().toLowerCase();


        const cards =
            container.querySelectorAll(
                ".item-card"
            );


        let visibleCount = 0;


        cards.forEach(card => {

            const searchText =
                card.dataset.search?.toLowerCase() || "";


            const matches =
                query === "" ||
                searchText.includes(query);


            card.style.display =
                matches
                    ? ""
                    : "none";


            if (matches) {

                visibleCount++;
            }
        });


        // -----------------------------------------------------
        // CATEGORY VISIBILITY
        // -----------------------------------------------------

        container
            .querySelectorAll(
                ".category-group"
            )
            .forEach(category => {

                const visibleCards =
                    [
                        ...category.querySelectorAll(
                            ":scope > .items-grid > .item-card"
                        )
                    ]
                        .filter(
                            card =>
                                card.style.display !== "none"
                        );


                category.style.display =
                    visibleCards.length > 0
                        ? ""
                        : "none";
            });


        // -----------------------------------------------------
        // TYPE VISIBILITY
        // -----------------------------------------------------

        container
            .querySelectorAll(
                ".type-group"
            )
            .forEach(type => {

                const visibleCards =
                    [
                        ...type.querySelectorAll(
                            ".item-card"
                        )
                    ]
                        .filter(
                            card =>
                                card.style.display !== "none"
                        );


                type.style.display =
                    visibleCards.length > 0
                        ? ""
                        : "none";
            });


        // -----------------------------------------------------
        // EMPTY / NO RESULTS
        // -----------------------------------------------------

        if (cards.length === 0) {

            emptyElement.hidden =
                false;

            noResultsElement.hidden =
                true;

        }
        else if (visibleCount === 0) {

            emptyElement.hidden =
                true;

            noResultsElement.hidden =
                false;

        }
        else {

            emptyElement.hidden =
                true;

            noResultsElement.hidden =
                true;
        }
    }


    // =========================================================
    // REFRESH SEARCH
    // =========================================================

    function refreshSearch() {

        filterContainer(
            availableContainer,
            availableSearch.value,
            availableEmpty,
            availableNoResults
        );


        filterContainer(
            unavailableContainer,
            unavailableSearch.value,
            unavailableEmpty,
            unavailableNoResults
        );
    }


    // =========================================================
    // SORT AVAILABLE
    // =========================================================

    function sortAvailableGroups() {

        sortContainer(
            availableContainer
        );
    }


    // =========================================================
    // SORT UNAVAILABLE
    // =========================================================

    function sortUnavailableGroups() {

        sortContainer(
            unavailableContainer
        );
    }


    // =========================================================
    // SORT CONTAINER
    // =========================================================

    function sortContainer(
        container
    ) {

        const typeGroups =
            [
                ...container.children
            ]
                .filter(
                    element =>
                        element.classList.contains(
                            "type-group"
                        )
                );


        // -----------------------------------------------------
        // Sort Types
        // -----------------------------------------------------

        typeGroups.sort(
            (a, b) =>
                Number(a.dataset.type) -
                Number(b.dataset.type)
        );


        typeGroups.forEach(
            typeGroup => {

                container.appendChild(
                    typeGroup
                );
            }
        );


        // -----------------------------------------------------
        // Sort Categories
        // -----------------------------------------------------

        typeGroups.forEach(
            typeGroup => {

                const categoriesContainer =
                    typeGroup.querySelector(
                        ":scope > .categories-container"
                    );


                if (!categoriesContainer)
                    return;


                const categories =
                    [
                        ...categoriesContainer.children
                    ]
                        .filter(
                            element =>
                                element.classList.contains(
                                    "category-group"
                                )
                        );


                categories.sort(
                    (a, b) =>
                        a.dataset.categoryName
                            .localeCompare(
                                b.dataset.categoryName
                            )
                );


                categories.forEach(
                    category => {

                        categoriesContainer.appendChild(
                            category
                        );
                    }
                );
            }
        );
    }


    // =========================================================
    // INITIAL STATE
    // =========================================================

    removeEmptyGroups();

    updateCounts();

    updateAllCategoryCounts();

    sortAvailableGroups();

    sortUnavailableGroups();

    refreshSearch();

});