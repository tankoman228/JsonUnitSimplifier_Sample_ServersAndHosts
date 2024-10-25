using JsonUnitSimplifier;
using ServersAndHosts.Repository;
using ServersAndHosts.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using ServersAndHosts;
using System.Threading;
using ServersAndHosts.Other;
using System;
using System.Text.RegularExpressions;

namespace TestForServersAndHostWithJSONUnitSimplifier
{
    [TestClass]
    public class Unit_AsyncManager
    {
        // Путь к файлам тестов
        private const string PATH = "C:\\Users\\Admin\\source\\repos\\ServersAndHosts\\Tests\\TestForServersAndHostWithJSONUnitSimplifier\\JSON\\";

        [TestMethod] 
        public void WithLib()
        {
            string errors = ""; // Для проверки на ошибки

            // Для аргумента типа Action<string>
            GenerateFunctions.AddFunc("action_error", i => new Action<string>(x => errors += x));

            // Создание датасета и выполнение тестов
            TestByJSON.TestObject<AsyncManager>(
               File.ReadAllText(PATH + "AsyncManager.json"), o =>
               { // Своя логика для проверки параллельности
                   o.TryAsyncOrReturnError(() => {                   
                    Thread.Sleep(100); throw new Exception(o.Where);
                });
            });
            Assert.AreEqual(errors, ""); // TryAsync выполнится асинхронно, значит, сейчас пусто
            Thread.Sleep(230); // TryAsync выполняется
            Assert.IsTrue(errors.Length > 30); // Ошибки есть? А если не найду?
        }

        [TestMethod]
        public void WithoutOfLibrary() // Без библиотеки
        {
            string errors = ""; 

            AsyncManager[] asyncManagers = {
                new AsyncManager("place A", x => errors += x),
                new AsyncManager("place A", x => errors += x),
                new AsyncManager("place B", x => errors += x),
                new AsyncManager("place B", x => errors += x),
                new AsyncManager("place C", x => errors += x),
                new AsyncManager("place C", x => errors += x),
            }; // Датасет

            //Проверка значения Where
            foreach (var asyncManager in asyncManagers)
            {
                Assert.IsTrue(Regex.IsMatch(asyncManager.Where, "place (A|B|C)"));
            }

            // Имитация параллельного выполнения
            foreach (var a in asyncManagers)
            {
                a.TryAsyncOrReturnError(() => {
                    Thread.Sleep(100);
                    throw new Exception(a.Where);  // Своя логика для проверки параллельности
                });
            }

            Assert.AreEqual(errors, ""); // TryAsync выполнится асинхронно, значит, сейчас пусто
            Thread.Sleep(230); // TryAsync выполняется
            Assert.IsTrue(errors.Length > 30); // Ошибки есть? А если не найду?
        }
    }
}
