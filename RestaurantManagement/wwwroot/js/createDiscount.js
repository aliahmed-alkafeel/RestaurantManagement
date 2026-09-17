
const addItemBtn =
    document.getElementById("addItemBtn");

const itemType =
    document.getElementById("itemType");

const itemCategory =
    document.getElementById("itemCategory");

const itemSelect =
    document.getElementById("itemSelect");

const itemsTableBody =
    document.getElementById("itemsTableBody");

const itemIdsContainer =
    document.getElementById("itemIdsContainer");


// ==================================================
// Discount Items
// ==================================================
//
// Contains all items currently selected
// for this discount.
//

let discountItems =
    Array.isArray(initialDiscountItems)
        ? initialDiscountItems.map(item => ({
            itemId: String(item.itemId),
            itemName: item.itemName,
            price: Number(item.price)
        }))
        : [];


// ==================================================
// Initial Render
// ==================================================

renderItems();

updateHiddenInputs();


// ==================================================
// Type Changed
// ==================================================

itemType.addEventListener(
    "change",
    async function () {

        const type =
            this.value;


        // ------------------------------------------
        // Reset Category
        // ------------------------------------------

        resetSelect(
            itemCategory,
            "Select Category"
        );


        // ------------------------------------------
        // Reset Item
        // ------------------------------------------

        resetSelect(
            itemSelect,
            "Select Item"
        );


        itemCategory.disabled = true;
        itemSelect.disabled = true;


        // ------------------------------------------
        // No Type
        // ------------------------------------------

        if (!type)
            return;


        try {

            const response =
                await fetch(
                    `/Dashboard/Items/GetCategoriesByType?type=${ encodeURIComponent(type) }`
                );


            if (!response.ok)
                throw new Error(
                    "Failed to load categories."
                );


            const categories =
                await response.json();


            // --------------------------------------
            // Add Categories
            // --------------------------------------

            categories.forEach(category => {

                const option =
                    document.createElement("option");


                option.value =
                    category.id;


                option.textContent =
                    category.categoryName;


                itemCategory.appendChild(option);

            });


            itemCategory.disabled =
                false;

        }
        catch (error) {

            console.error(error);

            alert(
                "Failed to load categories."
            );

        }

    }
);


// ==================================================
// Category Changed
// ==================================================

itemCategory.addEventListener(
    "change",
    async function () {

        const categoryId =
            this.value;


        // ------------------------------------------
        // Reset Item
        // ------------------------------------------

        resetSelect(
            itemSelect,
            "Select Item"
        );


        itemSelect.disabled =
            true;


        // ------------------------------------------
        // No Category
        // ------------------------------------------

        if (!categoryId)
            return;


        try {

            const response =
                await fetch(
                    `/Dashboard/Items/GetItemsByCategory?categoryId=${ encodeURIComponent(categoryId) }`
                );


            if (!response.ok)
                throw new Error(
                    "Failed to load items."
                );


            const items =
                await response.json();


            // --------------------------------------
            // Add Items To Select
            // --------------------------------------

            items.forEach(item => {

                // Do not show already selected items.

                if (
                    discountItems.some(
                        x =>
                            String(x.itemId) ===
                            String(item.id)
                    )
                ) {
                    return;
                }


                const option =
                    document.createElement("option");


                option.value =
                    item.id;


                option.textContent =
                    item.itemName;


                option.dataset.price =
                    item.price;


                itemSelect.appendChild(
                    option
                );

            });


            itemSelect.disabled =
                false;

        }
        catch (error) {

            console.error(error);

            alert(
                "Failed to load items."
            );

        }

    }
);


// ==================================================
// Add
// ==================================================

addItemBtn.addEventListener(
    "click",
    async function () {

        const type =
            itemType.value;


        const categoryId =
            itemCategory.value;


        const itemId =
            itemSelect.value;


        // ==================================================
        // Validation
        // ==================================================

        if (!type) {

            alert(
                "Please select a type."
            );

            return;
        }


        // Prevent double clicking.

        addItemBtn.disabled =
            true;


        try {

            // ==================================================
            // 1. Type only
            // ==================================================

            if (!categoryId) {

                const response =
                    await fetch(
                        `/Dashboard/Items/GetItemsByType?type=${encodeURIComponent(type)}`
                    );


                if (!response.ok)
                    throw new Error(
                        "Failed to load items by type."
                    );


                const items =
                    await response.json();


                addItems(
                    items
                );
            }


            // ==================================================
            // 2. Type + Category
            // ==================================================

            else if (!itemId) {

                const response =
                    await fetch(
                        `/Dashboard/Items/GetItemsByCategory?categoryId=${encodeURIComponent(categoryId)}`
                    );


                if (!response.ok)
                    throw new Error(
                        "Failed to load items by category."
                    );


                const items =
                    await response.json();


                addItems(
                    items
                );
            }


            // ==================================================
            // 3. Type + Category + Item
            // ==================================================

            else {

                const selectedOption =
                    itemSelect.options[
                        itemSelect.selectedIndex
                    ];


                if (!selectedOption) {

                    alert(
                        "Please select an item."
                    );

                    return;
                }


                const item = {

                    id:
                        itemId,

                    itemName:
                        selectedOption.textContent.trim(),

                    price:
                        Number(
                            selectedOption.dataset.price
                        )

                };


                addItems([
                    item
                ]);
            }


            // ==================================================
            // Update UI
            // ==================================================

            renderItems();

            updateHiddenInputs();


            // ==================================================
            // Reset Item Select
            // ==================================================

            resetSelect(
                itemSelect,
                "Select Item"
            );


            itemSelect.disabled =
                true;

        }
        catch (error) {

            console.error(error);

            alert(
                error.message ??
                "Failed to add items."
            );

        }
        finally {

            addItemBtn.disabled =
                false;

        }

    }
);


// ==================================================
// Add Items
// ==================================================
//
// Adds one or multiple items.
//
// Duplicate items are ignored.
//

function addItems(items) {

    items.forEach(item => {

        const itemId =
            String(item.id);


        // ------------------------------------------
        // Prevent Duplicate
        // ------------------------------------------

        const exists =
            discountItems.some(
                x =>
                    String(x.itemId) ===
                    itemId
            );


        if (exists)
            return;


        // ------------------------------------------
        // Add
        // ------------------------------------------

        discountItems.push({

            itemId:
                itemId,

            itemName:
                item.itemName,

            price:
                Number(item.price)

        });

    });

}


// ==================================================
// Render Items
// ==================================================

function renderItems() {

    itemsTableBody.innerHTML =
        "";


    discountItems.forEach(item => {

        const row =
            document.createElement("tr");


        row.innerHTML = `

    <td>
    ${ escapeHtml(item.itemName) }
            </td >

            <td>
                ${Number(item.price).toFixed(2)}
            </td>

            <td class="text-end">

                <button
                    type="button"
                    class="btn btn-sm btn-outline-danger delete-item"
                    data-item-id="${item.itemId}">

                    Delete

                </button>

            </td>

`;


        itemsTableBody.appendChild(
            row
        );

    });

}


// ==================================================
// Delete Button
// ==================================================

itemsTableBody.addEventListener(
    "click",
    function (event) {

        const deleteButton =
            event.target.closest(
                ".delete-item"
            );


        if (!deleteButton)
            return;


        const itemId =
            deleteButton.dataset.itemId;


        removeItem(
            itemId
        );

    }
);


// ==================================================
// Remove Item
// ==================================================

function removeItem(itemId) {

    discountItems =
        discountItems.filter(
            x =>
                String(x.itemId) !==
                String(itemId)
        );


    renderItems();

    updateHiddenInputs();


    // ------------------------------------------
    // Refresh current category items
    // ------------------------------------------

    if (
        itemCategory.value
    ) {

        itemCategory.dispatchEvent(
            new Event("change")
        );

    }

}


// ==================================================
// Hidden Inputs
// ==================================================

function updateHiddenInputs() {

    itemIdsContainer.innerHTML =
        "";


    discountItems.forEach(item => {

        const input =
            document.createElement("input");


        input.type =
            "hidden";


        input.name =
            "ItemIds";


        input.value =
            item.itemId;


        itemIdsContainer.appendChild(
            input
        );

    });

}


// ==================================================
// Reset Select
// ==================================================

function resetSelect(
    select,
    placeholder
) {

    select.innerHTML =
        "";


    const option =
        document.createElement("option");


    option.value =
        "";


    option.textContent =
        placeholder;


    select.appendChild(
        option
    );

}


// ==================================================
// Escape HTML
// ==================================================

function escapeHtml(value) {

    return String(value)

        .replaceAll(
            "&",
            "&amp;"
        )

        .replaceAll(
            "<",
            "&lt;"
        )

        .replaceAll(
            ">",
            "&gt;"
        )

        .replaceAll(
            '"',
            "&quot;"
        )

        .replaceAll(
            "'",
            "&#039;"
        );

}

