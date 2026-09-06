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

                option.dataset.discount =
                    item.discountPercentage ?? 0;

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
            selectedOption?.textContent?.trim();

        const price =
            Number(
                selectedOption?.dataset.price
            );

        const discount =
            Number(
                selectedOption?.dataset.discount
            ) || 0;

        const quantity =
            Number(itemQuantity.value);


        // ------------------------------------------
        // Validation
        // ------------------------------------------

        if (!itemId) {

            alert("Please select an item.");

            return;
        }


        if (!Number.isFinite(price)) {

            alert("Invalid item price.");

            return;
        }


        if (!Number.isFinite(quantity) || quantity < 1) {

            alert("Quantity must be at least 1.");

            return;
        }


        // ------------------------------------------
        // Find Existing Item
        // ------------------------------------------

        const existingItem =
            orderItems.find(
                x => x.itemId === itemId
            );


        // ------------------------------------------
        // Update Existing Item
        // ------------------------------------------

        if (existingItem) {

            existingItem.quantity += quantity;

            // Keep latest discount/price information
            existingItem.price = price;
            existingItem.discountPercentage = discount;

        }

        // ------------------------------------------
        // Create New Item
        // ------------------------------------------

        else {

            orderItems.push({

                itemId: itemId,

                itemName: itemName,

                price: price,

                discountPercentage: discount,

                quantity: quantity

            });

        }


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

        if (modal) {
            modal.hide();
        }

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


        const discount =
            Number(item.discountPercentage) || 0;


        const discountedPrice =
            discount > 0
                ? item.price * (1 - discount / 100)
                : item.price;


        const priceHtml =
            discount > 0
                ? `
                    <span class="original-price">
                        $${Number(item.price).toFixed(2)}
                    </span>

                    <span class="discounted-price">
                        $${discountedPrice.toFixed(2)}
                    </span>
                  `
                : `
                    <span class="discounted-price">
                        $${Number(item.price).toFixed(2)}
                    </span>
                  `;


        row.innerHTML = `

            <td>
                ${escapeHtml(item.itemName)}
            </td>

            <td>
                <div class="item-price-display">
                    ${priceHtml}
                </div>
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