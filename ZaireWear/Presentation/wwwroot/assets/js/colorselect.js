document.addEventListener("DOMContentLoaded", () => {
    document.addEventListener("click", (event) => {
        const button = event.target;
        if (!button.classList.contains("color")) return;

        const product = button.closest(".basket-wrapper") || button.closest(".product-details");

        if (product) {
            // Убираем выделение у всех цветов
            product.querySelectorAll(".color").forEach((color) => {
                color.classList.remove("selected");
            });

            // Выделяем выбранный цвет
            button.classList.add("selected");

            // Сохраняем выбранный цвет в скрытом поле
            const selectedColor = button.getAttribute("data-color");
            const colorInput = product.querySelector('input[name="selectedColor"]');
            if (colorInput) {
                colorInput.value = selectedColor;
            }
        }
    });
});