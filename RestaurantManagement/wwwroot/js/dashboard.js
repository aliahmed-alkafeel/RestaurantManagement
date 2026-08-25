document.getElementById("typeSelect").addEventListener("change", async function () {

    const type = this.value;
    const categorySelect = document.getElementById("categorySelect");
    categorySelect.innerHTML =
        '<option value="">Select Category</option>';

    if (!type)
        return;

    const response = await fetch(`/Dashboard/Items/GetCategoriesByType?type=${type}`);
    const categories = await response.json();
    categories.forEach(category => {
        console.log(type)
        console.log(category)
        const option = document.createElement("option");

        option.value = category.id;
        option.textContent = category.categoryName;

        categorySelect.appendChild(option);
    });
});