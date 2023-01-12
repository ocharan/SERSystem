using OpenQA.Selenium;

namespace SERTest;

public class Test01
{
    public void StartTest(WebDriver driver)
    {
        try
        {
            driver.Navigate().GoToUrl("https://localhost:7173/");
            driver.Manage().Window.Maximize();
            IWebElement inputUsuario = driver.FindElement((By.Id("inputUsuario")));
            IWebElement inputContraseña = driver.FindElement((By.Id("inputContraseña")));
            IWebElement btnLogin = driver.FindElement(By.Id("btnIniciarSesion"));
            inputUsuario.SendKeys("josuecg");
            inputContraseña.SendKeys("1234");
            btnLogin.Click();
            Console.WriteLine("Prueba 01 existosa");
        }
        catch (Exception e)
        {
            Console.WriteLine("Prueba 01 fallida");
        }
        
    }
}