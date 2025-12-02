using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;



using Lab2; // якщо в тебе інший namespace у Bus/CrudServiceAsync – заміни Lab2 на свій

namespace Lab2.Tests
{
    [TestClass]
    public class CrudServiceAsyncTests
    {
        private string GetTestFilePath()
        {
            // файл, у який буде зберігатися колекція під час тестів
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_buses.json");
        }

        [TestInitialize]
        public void TestInit()
        {
            // перед кожним тестом прибираємо старий файл, щоб тести не заважали одне одному
            var path = GetTestFilePath();
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private CrudServiceAsync<Bus> CreateService()
        {
            return new CrudServiceAsync<Bus>(GetTestFilePath());
        }

        [TestMethod]
        public async Task CreateAsync_ShouldAddNewItem()
        {
            // Arrange
            var service = CreateService();
            var bus = Bus.CreateNew();

            // Act
            var result = await service.CreateAsync(bus);

            // Assert
            Assert.IsTrue(result, "CreateAsync має повертати true для нового елемента.");
        }

        [TestMethod]
        public async Task ReadAsync_ShouldReturnItemById()
        {
            // Arrange
            var service = CreateService();
            var bus = Bus.CreateNew();
            await service.CreateAsync(bus);

            // Act
            var readBus = await service.ReadAsync(bus.Id);

            // Assert
            Assert.IsNotNull(readBus, "ReadAsync має повертати елемент, якщо він існує.");
            Assert.AreEqual(bus.Id, readBus.Id, "Id збереженого та прочитаного елементів мають збігатися.");
        }

        [TestMethod]
        public async Task ReadAllAsync_ShouldReturnAllItems()
        {
            // Arrange
            var service = CreateService();
            await service.CreateAsync(Bus.CreateNew());
            await service.CreateAsync(Bus.CreateNew());
            await service.CreateAsync(Bus.CreateNew());

            // Act
            var all = await service.ReadAllAsync();
            var list = all.ToList();

            // Assert
            Assert.AreEqual(3, list.Count, "ReadAllAsync має повертати всі додані елементи.");
        }

        [TestMethod]
        public async Task ReadAllAsync_WithPaging_ShouldReturnCorrectAmount()
        {
            // Arrange
            var service = CreateService();

            for (int i = 0; i < 10; i++)
            {
                await service.CreateAsync(Bus.CreateNew());
            }

            int page = 2;
            int amount = 3;

            // Act
            var pageItems = await service.ReadAllAsync(page, amount);
            var list = pageItems.ToList();

            // Assert
            Assert.AreEqual(amount, list.Count,
                "Друга сторінка має містити вказану кількість елементів (якщо їх достатньо).");
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateExistingItem()
        {
            // Arrange
            var service = CreateService();
            var bus = Bus.CreateNew();
            await service.CreateAsync(bus);

            int oldCapacity = bus.Capacity;
            bus.Capacity = oldCapacity + 10;

            // Act
            var updated = await service.UpdateAsync(bus);
            var readBus = await service.ReadAsync(bus.Id);

            // Assert
            Assert.IsTrue(updated, "UpdateAsync має повертати true при успішному оновленні.");
            Assert.IsNotNull(readBus);
            Assert.AreEqual(bus.Capacity, readBus.Capacity, "Після оновлення значення поля Capacity має змінитися.");
        }

        [TestMethod]
        public async Task RemoveAsync_ShouldDeleteItem()
        {
            // Arrange
            var service = CreateService();
            var bus = Bus.CreateNew();
            await service.CreateAsync(bus);

            // Act
            var removed = await service.RemoveAsync(bus);
            var readBus = await service.ReadAsync(bus.Id);

            // Assert
            Assert.IsTrue(removed, "RemoveAsync має повертати true при успішному видаленні.");
            Assert.IsNull(readBus, "Після RemoveAsync елемент не повинен повертатися з ReadAsync.");
        }

        [TestMethod]
        public async Task SaveAsync_ShouldCreateFile()
        {
            // Arrange
            var service = CreateService();
            await service.CreateAsync(Bus.CreateNew());
            var path = GetTestFilePath();

            // Act
            var saveResult = await service.SaveAsync();

            // Assert
            Assert.IsTrue(saveResult, "SaveAsync має повертати true при успішному збереженні.");
            Assert.IsTrue(File.Exists(path), "Після SaveAsync файл з колекцією має існувати.");
        }
    }
}
