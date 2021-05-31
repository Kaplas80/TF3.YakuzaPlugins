#load "nuget:?package=PleOps.Cake&version=0.4.2"

Task("Define-Project")
    .Description("Fill specific project information")
    .Does<BuildInfo>(info =>
{
    info.AddLibraryProjects("TF3.YarhlPlugin.YakuzaCommon");
    info.AddLibraryProjects("TF3.YarhlPlugin.YakuzaKiwami2");
    info.AddTestProjects("TF3.Tests.Yakuza");
    info.AddTestProjects("TF3.Tests.YakuzaKiwami2");
    info.AddApplicationProjects("src/Apps/FontSpacingEditor/FontSpacingEditor.csproj");

    info.CoverageTarget = 75;
    info.PreviewNuGetFeed = "https://nuget.pkg.github.com/Kaplas80/index.json";
    info.PreviewNuGetFeedToken = info.GitHubToken;
    info.StableNuGetFeed = "https://nuget.pkg.github.com/Kaplas80/index.json";
    info.StableNuGetFeedToken = info.GitHubToken;
});

Task("Default")
    .IsDependentOn("Stage-Artifacts");

string target = Argument("target", "Default");
RunTarget(target);
