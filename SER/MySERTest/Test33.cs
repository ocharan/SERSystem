using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test33
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
    public void test33()
    {
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecg");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnCuerpoAcademico")).Click();
        driver.FindElement(By.Id("btnProyectosInv")).Click();
        driver.FindElement(By.Id("btnRegistrarProyecto")).Click();
        
        driver.FindElement(By.Id("select_tipo")).Click();
        {
            var dropdown = driver.FindElement(By.Id("select_tipo"));
            dropdown.FindElement(By.XPath("//option[. = 'PLADEA']")).Click();
        }
        
        driver.FindElement(By.Id("btn_Confirmar")).Click();
        
        driver.FindElement(By.Id("inputAccion")).SendKeys("Acción pladea de ingeniería de software");
        
        driver.FindElement(By.Id("yearStart")).Click();
        {
            var dropdown1 = driver.FindElement(By.Id("yearStart"));
            dropdown1.FindElement(By.XPath("//option[. = '2018']")).Click();
        }
        
        driver.FindElement(By.Id("yearEnd")).Click();
        {
            var dropdown2 = driver.FindElement(By.Id("yearEnd"));
            dropdown2.FindElement(By.Id("2022")).Click();
        }

        driver.FindElement(By.Id("pladeaRegistrar_ObjetivoGeneral")).SendKeys("Objetivo de PLADEA de ingeniería de software");
        driver.FindElement(By.Id("btnGuardarPladea")).Click();
    }
}