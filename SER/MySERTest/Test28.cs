using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test28
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
    public void test28()
    {
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecg");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnCuerpoAcademico")).Click();
        driver.FindElement(By.Id("btnLgacs")).Click();
        driver.FindElement(By.Id("btnRegistrarLgac")).Click();
        
        driver.FindElement(By.Id("input_nombreLgac")).SendKeys("LGAC de ingeniería de software");
        driver.FindElement(By.Id("cuerpos_list")).Click();
        {
            var dropdown = driver.FindElement(By.Id("cuerpos_list"));
            dropdown.FindElement(By.XPath("//option[. = 'Cuerpo académico de ingeniería de software y tecnologias']")).Click();
        }

        driver.FindElement(By.Id("input_descripcion")).SendKeys("Descripción de LGAC de ingeniería de software. ");
        
        driver.FindElement(By.Id("btnGuardarLgac")).Click();
    }
}