document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".color-options").forEach((options) => {
        options.addEventListener("click", (event) => {
            const button = event.target;
            if (!button.classList.contains("color")) return;

            const product = button.closest(".basket-wrapper") || button.closest(".product-details");

            if (product) {
                product.querySelectorAll(".color").forEach((color) => {
                    color.classList.remove("selected");
                });

                button.classList.add("selected");
            }
        });
    });
});
