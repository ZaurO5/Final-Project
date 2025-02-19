document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".size-options").forEach((options) => {
        options.addEventListener("click", (event) => {
            const button = event.target;
            if (!button.classList.contains("size")) return;

            const product = button.closest(".product-card");

            if (product) {
                product.querySelectorAll(".size").forEach((size) => {
                    size.classList.remove("selected");
                });

                button.classList.add("selected");
            }
        });
    });
});
