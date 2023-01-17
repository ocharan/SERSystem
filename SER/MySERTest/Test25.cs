using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test25
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
    public void test25()
    {
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecg");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnCuerpoAcademico")).Click();
        driver.FindElement(By.Id("btnCuerposAcademicos")).Click();
        driver.FindElement(By.Id("btnAñadirIntegrante&1040")).Click();
        driver.FindElement(By.Id("btnGuardar")).Click();
        driver.FindElement(By.Id("btnAsignarMiembro22")).Click();
        
        string mensajeRecibido = driver.FindElement(By.Id("seleccionError")).Text;
        string mensajeEsperado = "Debe seleccionar un tipo de integrante";
        Assert.AreEqual(mensajeRecibido, mensajeEsperado);
    }
}