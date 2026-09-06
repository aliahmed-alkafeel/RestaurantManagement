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

let discountItems = [...initialDiscountItems];

renderItems();
updateHiddenInputs();


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

        const type = this.value;


        resetSelect(
            itemCategory,
            "Select Category"
        );


        resetSelect(
            itemSelect,
            "Select Item"
        );


        itemCategory.disabled = true;
        itemSelect.disabled = true;


        if (!type)
            return;


        try {

            const response = await fetch(
                `/Dashboard/Items/GetCategoriesByType?type=${type}`
            );


            if (!response.ok)
                throw new Error(
                    "Failed to load categories."
                );


            const categories =
                await response.json();


            categories.forEach(category => {

                const option =
                    document.createElement("option");


                option.value =
                    category.id;


                option.textContent =
                    category.categoryName;


                itemCategory.appendChild(option);

            });


            itemCategory.disabled = false;

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

        const categoryId = this.value;


        resetSelect(
            itemSelect,
            "Select Item"
        );


        itemSelect.disabled = true;


        if (!categoryId)
            return;


        try {

            const response = await fetch(
                `/Dashboard/Items/GetItemsByCategory?categoryId=${categoryId}`
            );


            if (!response.ok)
                throw new Error(
                    "Failed to load items."
                );


            const items =
                await response.json();


            items.forEach(item => {

                // Don't add an item that is
                // already selected.

                if (
                    discountItems.some(
                        x => x.itemId === item.id
                    )
                )
                    return;


                const option =
                    document.createElement("option");


                option.value =
                    item.id;


                option.textContent =
                    item.itemName;


                option.dataset.price =
                    item.price;


                itemSelect.appendChild(option);

            });


            itemSelect.disabled = false;

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
// Add Item
// ==================================================

addItemBtn.addEventListener(
    "click",
    function () {

        const itemId =
            itemSelect.value;


        const selectedOption =
            itemSelect.options[
            itemSelect.selectedIndex
            ];


        const itemName =
            selectedOption?.textContent;


        const price =
            Number(
                selectedOption?.dataset.price
            );


        // ------------------------------------------
        // Validation
        // ------------------------------------------

        if (!itemId) {

            alert(
                "Please select an item."
            );

            return;
        }


        // ------------------------------------------
        // Prevent Duplicate
        // ------------------------------------------

        const existingItem =
            discountItems.find(
                x => x.itemId === itemId
            );


        if (existingItem) {

            alert(
                "This item is already selected."
            );

            return;
        }


        // ------------------------------------------
        // Create Item
        // ------------------------------------------

        const item = {

            itemId: itemId,

            itemName: itemName,

            price: price

        };


        discountItems.push(item);


        // ------------------------------------------
        // Update UI
        // ------------------------------------------

        renderItems();

        updateHiddenInputs();


        // ------------------------------------------
        // Remove From Select
        // ------------------------------------------

        selectedOption.remove();

        itemSelect.value = "";

    }
);


// ==================================================
// Render Items
// ==================================================

function renderItems() {

    itemsTableBody.innerHTML = "";


    discountItems.forEach(item => {

        const row =
            document.createElement("tr");


        row.innerHTML = `

            <td>
                ${escapeHtml(item.itemName)}
            </td>

            <td>
                ${item.price}
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


        itemsTableBody.appendChild(row);

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


        removeItem(itemId);

    }
);


// ==================================================
// Remove Item
// ==================================================

function removeItem(itemId) {

    discountItems =
        discountItems.filter(
            x => x.itemId !== itemId
        );


    renderItems();

    updateHiddenInputs();

}


// ==================================================
// Hidden Inputs
// ==================================================

function updateHiddenInputs() {

    itemIdsContainer.innerHTML = "";


    discountItems.forEach(item => {

        const input =
            document.createElement("input");


        input.type = "hidden";


        input.name = "ItemIds";


        input.value = item.itemId;


        itemIdsContainer.appendChild(input);

    });

}


// ==================================================
// Reset Select
// ==================================================

function resetSelect(
    select,
    placeholder
) {

    select.innerHTML = "";


    const option =
        document.createElement("option");


    option.value = "";

    option.textContent =
        placeholder;


    select.appendChild(option);

}


// ==================================================
// Escape HTML
// ==================================================

function escapeHtml(value) {

    return String(value)
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#039;");

}