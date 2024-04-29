using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTestForKonturStend;

public class SeleniumTest
{
    public ChromeDriver driver;

    [SetUp]
    public void SetUp()
    {
        //Предусловия
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox","--disable-extensions"); 
        options.AddArguments("--headless"); //Вынес отдельно,чтобы была возможность быстро скрыть браузер
        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        Authorization();
    }

    [Test] //Тест на авторизацию с валидными логином и паролем
    public void Authorization1()
    {
        driver.Url.Should().Be("https://staff-testing.testkontur.ru/news");
    }
    
    [Test] //Тест на создание мероприятия
    public void CreateEvent()
    {
        // Перейти на страницу мероприятия
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/events");
        
        //Явное ожидание, чтобы можно было нажать на кнопку
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='AddButton']")));
        
        // Клик на кнопку "Создать"
        var create = driver.FindElement(By.CssSelector("[data-tid='AddButton']"));
        create.Click(); 
        
        // Указать "Название"
        var nameEvent = driver.FindElement(By.CssSelector("[data-tid='Name']"));
        nameEvent.SendKeys("Хочу в Контур");
        
        // Указать "ИНН"
        var inn = driver.FindElement(By.CssSelector("[placeholder='Введите ИНН']"));
        inn.SendKeys("000000000000");
        
        // Указать признак "Полный день" 
        var allDay = driver.FindElement(By.CssSelector("[data-tid='AllDay']"));
        allDay.Click();
        
        // Клик на кнопку "Создать"
        var createButton = driver.FindElement(By.CssSelector("[data-tid='CreateButton']"));
        createButton.Click(); 
        
        //Если мероприятие создано, появится текст "Управление мероприятием". Ищу его для проверки
        string namesearch = driver.FindElement(By.XPath("//*[contains(text(),'Управление мероприятием')]")).Text;
        namesearch.Should().Contain("Управление мероприятием");
        
        //Ищу кнопку удалить и нажимаю на неё
        var delbatton = driver.FindElement(By.CssSelector("[data-tid='DeleteButton']"));
        delbatton.Click();

        //В подтверждающем окне так же нахожу кнопку удаления и нажимаю её
        var delclick = driver.FindElement(By.CssSelector("[data-tid='ModalPageFooter'] button"));
        delclick.Click();
    }

    public void Authorization()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        var login = driver.FindElement(By.Id("Username")); 
        login.SendKeys("mkof@list.ru");
        var password = driver.FindElement(By.Name("Password")); 
        password.SendKeys("Gang2020!!");
        var enter = driver.FindElement(By.Name("button")); 
        enter.Click();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/news"));
    }

    [TearDown] 
    public void TearDown() 
    { 
        driver.Close();
        driver.Quit(); 
    } 
}
