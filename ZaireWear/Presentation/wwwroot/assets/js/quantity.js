//<script>
//    document.addEventListener('DOMContentLoaded', function () {
//        const quantityInput = document.getElementById('quantity');
//    const quantityError = document.getElementById('quantity-error');
//    const stockCount = @Model.StockCount; // Получаем значение StockCount из модели

//    quantityInput.addEventListener('input', function () {
//            const selectedQuantity = parseInt(this.value);

//            if (selectedQuantity > stockCount) {
//        quantityError.style.display = 'block';
//    this.value = stockCount; // Устанавливаем значение равным StockCount
//            } else {
//        quantityError.style.display = 'none';
//            }
//        });

//    // Обработка кнопки "Add to Shopping Bag"
//    const addToBasketButton = document.querySelector('.add-to-basket');
//    addToBasketButton.addEventListener('click', function (e) {
//            const selectedQuantity = parseInt(quantityInput.value);

//            if (selectedQuantity > stockCount) {
//        e.preventDefault(); // Отменяем действие по умолчанию
//    quantityError.style.display = 'block';
//    alert('Quantity cannot exceed available stock.');
//            }
//        });
//    });
//</script>