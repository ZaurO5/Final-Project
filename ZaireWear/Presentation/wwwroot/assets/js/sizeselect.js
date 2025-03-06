document.addEventListener("DOMContentLoaded", () => {
    document.addEventListener("click", (event) => {
        const button = event.target;
        if (!button.classList.contains("size")) return;

        const product = button.closest(".product-card") || button.closest(".product-details");

        if (product) {
            // Убираем выделение у всех размеров
            product.querySelectorAll(".size").forEach((size) => {
                size.classList.remove("selected");
            });

            // Выделяем выбранный размер
            button.classList.add("selected");

            // Сохраняем выбранный размер в скрытом поле
            const selectedSize = button.getAttribute("data-size");
            const sizeInput = product.querySelector('input[name="selectedSize"]');
            if (sizeInput) {
                sizeInput.value = selectedSize;
            }
        }
    });
});