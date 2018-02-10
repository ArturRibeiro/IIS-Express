
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Scorponok.Web.Api.Unit.Test.Integration.Helper;


[SetUpFixture]
public class Global
{
    #region Atributos
    const string TEST_SOL = "Scorponok.sln";

    private KeyValuePair<string, int> _hostScorponokWebApi = new KeyValuePair<string, int>("Scorponok.Web.Api", 53290);

    private IISExpress _iisExpressWebApi;
    #endregion

    [OneTimeSetUp]
    public void Setup()
    {
        var pathRoot = GetRootPath();

        var fullPathEsb = GetPath(pathRoot, _hostScorponokWebApi.Key);
        if (fullPathEsb == null) throw new InvalidOperationException($"Não foi possível obter o diretório: {_hostScorponokWebApi.Value} raiz da solução de testes de integração");
        _iisExpressWebApi = IISExpress.Start(fullPathEsb, _hostScorponokWebApi);
    }

    [OneTimeTearDown]
    public void Teardown() => _iisExpressWebApi?.Stop();

    #region Métodos Privados

    private string GetRootPath()
    {
        var currentDir = AppDomain.CurrentDomain.BaseDirectory;
        string rootPath = null;
        while (rootPath == null)
        {
            var dir = Directory.GetParent(currentDir);
            if (dir.GetFiles(TEST_SOL).Any())
                rootPath = dir.Parent.FullName;
            currentDir = dir.FullName;
        }
        return rootPath;
    }

    private static string GetPath(string rootPath, string projectName)
    {
        var dir = new DirectoryInfo(rootPath);
        if (dir.GetFiles(projectName).Any())
            return dir.FullName;

        return dir.GetDirectories()
            .Select(subDir => GetPath(subDir.FullName, projectName))
            .FirstOrDefault(res => res != null);
    }

    #endregion

}
