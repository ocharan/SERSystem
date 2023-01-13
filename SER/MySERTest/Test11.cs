using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MySER;

public class Test11
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
    public void test11()
    {
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecapi");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnDocentes")).Click();
        driver.FindElement(By.Id("btnRegistrarDocente")).Click();

        driver.FindElement(By.Id("inputNombre")).SendKeys("Juan Diego");
        driver.FindElement(By.Id("inputApellidoPaterno")).SendKeys("Gonzalez");
        driver.FindElement(By.Id("inputApellidoMaterno")).SendKeys("Del Olmo");
        driver.FindElement(By.Id("input_numeroPersonal")).SendKeys("12345");
        driver.FindElement(By.Id("input_Contraseña")).SendKeys("787878");
        driver.FindElement(By.Id("input_ContraseñaRepeat")).SendKeys("787878");
        driver.FindElement(By.Id("btnGuardarDocente")).Click();

        string mensajeObetnido = driver.FindElement(By.Id("alertError")).Text;
        string mensajeEsperado = "El docente que inténtas registrar ya existe";
        Assert.AreEqual(mensajeEsperado, mensajeObetnido);
    }
}