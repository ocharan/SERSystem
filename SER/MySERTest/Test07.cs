using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace MySER;

public class Test07
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
    public void test07()
    {
        driver.Navigate().GoToUrl("https://localhost:7173/");
        driver.Manage().Window.Size = new System.Drawing.Size(1846, 1053);
        driver.FindElement(By.Id("inputUsuario")).SendKeys("josuecapi");
        driver.FindElement(By.Id("inputContraseña")).SendKeys("1234");
        driver.FindElement(By.Id("btnIniciarSesion")).Click();
        driver.FindElement(By.Id("btnAlumnos")).Click();
        driver.FindElement(By.Id("btnRegistrarAlumno")).Click();

        driver.FindElement(By.Id("inputNombreAlumno")).SendKeys("Diego Fernando");
        driver.FindElement(By.Id("inputApellidoPaterno")).SendKeys("Fernandez");
        driver.FindElement(By.Id("inputApellidoMaterno")).SendKeys("García");
        driver.FindElement(By.Id("inputMatricula")).SendKeys("s22345077");
        driver.FindElement(By.Id("inputCorreoAlumno")).SendKeys("fergarc@hotmail.com");
        driver.FindElement(By.Id("btnGuardarAlumno")).Click();
    }
}