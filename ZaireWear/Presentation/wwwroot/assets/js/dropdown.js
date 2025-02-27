document.addEventListener("DOMContentLoaded", function () {
    const catalogBtn = document.getElementById("catalog-btn");
    const dropdownMenu = document.querySelector(".dropdown-menu");

    catalogBtn.addEventListener("click", function (event) {
        event.preventDefault();
        dropdownMenu.classList.toggle("active");
    });

    document.addEventListener("click", function (event) {
        if (!catalogBtn.contains(event.target) && !dropdownMenu.contains(event.target)) {
            dropdownMenu.classList.remove("active");
        }
    });
});
