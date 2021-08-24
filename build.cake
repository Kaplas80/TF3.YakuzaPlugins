#load "nuget:?package=PleOps.Cake&version=0.4.2"

Task("Define-Project")
    .Description("Fill specific project information")
    .Does<BuildInfo>(info =>
{
    info.CoverageTarget = 75;
    info.PreviewNuGetFeed = "https://nuget.pkg.github.com/Kaplas80/index.json";
    info.PreviewNuGetFeedToken = info.GitHubToken;
    info.StableNuGetFeed = "https://nuget.pkg.github.com/Kaplas80/index.json";
    info.StableNuGetFeedToken = info.GitHubToken;

    info.AddApplicationProjects("TF3.YarhlPlugin.YakuzaCommon");
    info.AddApplicationProjects("TF3.YarhlPlugin.YakuzaKiwami2");
    info.AddLibraryProjects("TF3.YarhlPlugin.YakuzaCommon");
    info.AddLibraryProjects("TF3.YarhlPlugin.YakuzaKiwami2");
    info.AddTestProjects("TF3.Tests.Yakuza");
    info.AddTestProjects("TF3.Tests.YakuzaKiwami2");
});

Task("Default")
    .IsDependentOn("Stage-Artifacts");

string target = Argument("target", "Default");
RunTarget(target);
