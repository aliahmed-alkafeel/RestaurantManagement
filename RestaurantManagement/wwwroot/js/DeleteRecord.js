
document.addEventListener("DOMContentLoaded", function () {

// =====================================================
// Selected Row
// =====================================================

let selectedRow = null;


// =====================================================
// Delete
// =====================================================

document.querySelectorAll(".delete-btn")
.forEach(button => {

button.addEventListener("click", function () {

// Remove previous selection
if (selectedRow) {
selectedRow.classList.remove("modal-selected");
}


// Get current row
selectedRow = this.closest(".data-row");


// Highlight current row
if (selectedRow) {
selectedRow.classList.add("modal-selected");
}


// Set form action
const url = this.dataset.url;

document.getElementById("deleteForm").action = url;

});

});


// =====================================================
// Remove Selection When Modal Closes
// =====================================================

document.querySelectorAll(".modal")
.forEach(modal => {

modal.addEventListener("hidden.bs.modal", function () {

if (selectedRow) {

selectedRow.classList.remove("modal-selected");

selectedRow = null;

}

});

});

});
