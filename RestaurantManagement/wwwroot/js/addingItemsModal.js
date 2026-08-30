const addItemBtn = document.getElementById("addItemBtn");

const itemType = document.getElementById("itemType");
const itemCategory = document.getElementById("itemCategory");
const itemSelect = document.getElementById("itemSelect");
const itemQuantity = document.getElementById("itemQuantity");

const confirmAddItem = document.getElementById("confirmAddItem");

const itemsTableBody =
    document.getElementById("itemsTableBody");

const itemOrdersContainer =
    document.getElementById("itemOrdersContainer");


// ==================================================
// Order Items
// ==================================================
//
// This contains ALL items currently in the order.
// Existing items + newly added items.
//

let orderItems = initialOrderItems;


// ==================================================
// Initial Render
// ==================================================

renderItems();

updateHiddenInputs();


// ==================================================
// Open Modal
// ==================================================

const addItemModal =
    document.getElementById("addItemModal");


if (addItemModal) {

    addItemModal.addEventListener(
        "show.bs.modal",
        function () {

            resetItemModal();

        }
    );

}


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

confirmAddItem.addEventListener(
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


        const quantity =
            Number(itemQuantity.value);


        // ------------------------------------------
        // Validation
        // ------------------------------------------

        if (!itemId) {

            alert(
                "Please select an item."
            );

            return;
        }


        if (quantity < 1) {

            alert(
                "Quantity must be at least 1."
            );

            return;
        }


        // ------------------------------------------
        // Prevent Duplicate
        // ------------------------------------------

        const alreadyExists =
            orderItems.some(
                x => x.itemId === itemId
            );


        if (alreadyExists) {

            alert(
                "This item has already been added."
            );

            return;
        }


        // ------------------------------------------
        // Create Item
        // ------------------------------------------

        const item = {

            itemId: itemId,

            itemName: itemName,

            price: price,

            quantity: quantity

        };


        // ------------------------------------------
        // Add To Array
        // ------------------------------------------

        orderItems.push(item);


        // ------------------------------------------
        // Update UI
        // ------------------------------------------

        renderItems();

        updateHiddenInputs();


        // ------------------------------------------
        // Close Modal
        // ------------------------------------------

        const modal =
            bootstrap.Modal.getInstance(
                addItemModal
            );


        if (modal)
            modal.hide();

    }
);


// ==================================================
// Render Items
// ==================================================

function renderItems() {

    itemsTableBody.innerHTML = "";


    orderItems.forEach(item => {

        const row =
            document.createElement("tr");


        row.innerHTML = `

            <td>
                ${escapeHtml(item.itemName)}
            </td>

            <td>
                ${item.price}
            </td>

            <td>
                ${item.quantity}
            </td>

            <td>

                <button
                    type="button"
                    class="action delete-action delete-item"
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

    orderItems =
        orderItems.filter(
            x => x.itemId !== itemId
        );


    renderItems();

    updateHiddenInputs();

}


// ==================================================
// Hidden Inputs
// ==================================================

function updateHiddenInputs() {

    itemOrdersContainer.innerHTML = "";


    orderItems.forEach(
        (item, index) => {

            itemOrdersContainer.innerHTML += `

                <input type="hidden"
                       name="ItemOrders[${index}].ItemId"
                       value="${item.itemId}" />

                <input type="hidden"
                       name="ItemOrders[${index}].ItemName"
                       value="${escapeHtml(item.itemName)}" />

                <input type="hidden"
                       name="ItemOrders[${index}].Price"
                       value="${item.price}" />

                <input type="hidden"
                       name="ItemOrders[${index}].Quantity"
                       value="${item.quantity}" />

            `;

        }
    );

}


// ==================================================
// Reset Modal
// ==================================================

function resetItemModal() {

    itemType.value = "";


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


    itemQuantity.value = 1;

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