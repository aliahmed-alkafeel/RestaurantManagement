const typeSelect =
    document.getElementById("typeSelect");

const categorySelect =
    document.getElementById("categorySelect");


const selectedCategoryId =
    categorySelect.dataset.selectedCategory;


// ==================================================
// Load Categories
// ==================================================

async function loadCategories(isInitialLoad = false) {

    const type =
        typeSelect.value;


    categorySelect.innerHTML =
        '<option value="default">Select Category</option>';


    if (!type)
        return;


    try {

        const response =
            await fetch(
                `/Dashboard/Items/GetCategoriesByType?type=${encodeURIComponent(type)}`
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

            categorySelect.appendChild(option);

        });


        // Select the old category

        if (selectedCategoryId && isInitialLoad) {

            categorySelect.value =
                selectedCategoryId;

        }
        else {
           
        categorySelect.value = "default";
        }
        

    }
    catch (error) {

        console.error(error);

    }
}


// ==================================================
// Events
// ==================================================

typeSelect.addEventListener(
    "change",
()=>loadCategories(false)
);


// ==================================================
// Page Load
// ==================================================

loadCategories(true);