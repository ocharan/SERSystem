using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test55
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
    public void test55()
    {
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecg");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnExperienciaRecepcional")).Click();
        driver.FindElement(By.Id("btnTrabajos")).Click();
        driver.FindElement(By.Id("btnAsignarSinodales&2017")).Click();
        
        driver.FindElement(By.Id("select_tipo")).Click();
        {
            var dropdown = driver.FindElement(By.Id("select_tipo"));
            dropdown.FindElement(By.XPath("//option[. = 'Co-director']")).Click();
        }
        
        driver.FindElement(By.Id("btnAsignar2016")).Click();
        driver.FindElement(By.Id("btnAsignar2017")).Click();
        driver.FindElement(By.Id("btnAsignar2018")).Click();
        
        driver.FindElement(By.Id("btn_Guardar")).Click();

        string mensajeRecibido = driver.FindElement(By.Id("errorSinDirector")).Text;
        string mensajeEsperado = "El trabajo recepcional debe tener al menos un director.";
        
        Assert.AreEqual(mensajeEsperado, mensajeRecibido);

    }
}