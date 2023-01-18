using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test45
{
    private IWebDriver driver;
    public IDictionary<string, object> vars {get; private set;}
    private IJavaScriptExecutor js;
    [SetUp]
    public void SetUp() {
        FirefoxOptions firefoxOptions = new FirefoxOptions();
        firefoxOptions.AcceptInsecureCertificates = true;
        driver = new FirefoxDriver(firefoxOptions);
        js = (IJavaScriptExecutor)driver;
        vars = new Dictionary<string, object>();
    }
    [TearDown]
    protected void TearDown() {
        driver.Quit();
    }

    [Test]
    public void test45()
    {
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecg");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnExperienciaRecepcional")).Click();
        driver.FindElement(By.Id("btnSinodales")).Click();
        driver.FindElement(By.Id("btnRegistrarSinodal")).Click();
        
        driver.FindElement(By.Id("select_tipo")).Click();
        {
            var dropdown = driver.FindElement(By.Id("select_tipo"));
            dropdown.FindElement(By.XPath("//option[. = 'Sinodal externo']")).Click();
        }
        
        driver.FindElement(By.Id("btnConfirmar")).Click();

        
        driver.FindElement(By.Id("inputNombreSinodal")).SendKeys("Francisco");
        driver.FindElement(By.Id("inputApellidoPaterno")).SendKeys("Orozco");
        driver.FindElement(By.Id("inputApellidoMaterno")).SendKeys("Rios");

        driver.FindElement(By.Id("cuerpos_list")).Click();
        {
            var dropdown = driver.FindElement(By.Id("cuerpos_list"));
            dropdown.FindElement(By.XPath("//option[. = 'Instituto de inteligencia artificial y fisica']")).Click();
        }
        
        driver.FindElement(By.Id("inputCorreo")).SendKeys("Francisoroz@gmail.com");
        driver.FindElement(By.Id("inputTelefono")).SendKeys("2255448899 ");

        
        driver.FindElement(By.Id("btnGuardarSinodal")).Click();
    }
}