using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test51
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
    public void test51()
    {
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecg");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnExperienciaRecepcional")).Click();
        driver.FindElement(By.Id("btnTrabajos")).Click();
        driver.FindElement(By.Id("btnModificar&2017")).Click();

        driver.FindElement(By.Id("TrabajoRecepcional_Duracion")).Click();
        {
            var dropdown = driver.FindElement(By.Id("TrabajoRecepcional_Duracion"));
            dropdown.FindElement(By.XPath("//option[. = '6 meses']")).Click();
        }

        driver.FindElement(By.Id("TrabajoRecepcional_Modalidad")).Click();
        {
            var dropdown = driver.FindElement(By.Id("TrabajoRecepcional_Modalidad"));
            dropdown.FindElement(By.XPath("//option[. = 'Tesis']")).Click();
        }

        driver.FindElement(By.Id("estadoExperiencia")).Click();
        {
            var dropdown = driver.FindElement(By.Id("selectTipo"));
            dropdown.FindElement(By.XPath("//option[. = 'Experiencia recepcional']")).Click();
        }
        
        driver.FindElement(By.Id("selectProyecto")).Click();
        {
            var dropdown = driver.FindElement(By.Id("selectProyecto"));
            dropdown.FindElement(By.XPath("//option[. = 'Instituto de inteligencia artificial y fisica']")).Click();
        }


        driver.FindElement(By.Id("btnGuardarProyecto")).Click();
    }
}