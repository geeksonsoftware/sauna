var target = Argument("target", "Build");
var configuration = Argument("configuration", "RPI");
var runtime = Argument("runtime", "linux-arm");

string host="";
string hostUser="";
string hostDestination="";

if(target.Equals("Deploy", StringComparison.InvariantCultureIgnoreCase)){
  host=Argument<string>("host");
  hostUser=Argument("user","pi");
  hostDestination=Argument("destination","/var/opt/sauna");
}

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    DotNetCoreClean("./Sauna.sln");
});

Task("Restore")
    .Does(() =>
{
    DotNetCoreRestore();
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{

    DotNetCoreBuild("./Sauna.sln", new DotNetCoreBuildSettings
    {        
        Configuration = configuration,
        Runtime = runtime
    });
});

Task("Publish")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
    DotNetCorePublish("./Sauna.Server", new DotNetCorePublishSettings
    {        
        Configuration = configuration,
        Runtime = runtime
    });
});

Task("Deploy")
    .IsDependentOn("Publish")
    .Does(() =>
{
    var source = $"./Sauna.Server/bin/{configuration}/netcoreapp3.1/{runtime}/publish/";
    var destination = $"{hostUser}@{host}:{hostDestination}";

    var exitCodeWithArgument = StartProcess("rsync", new ProcessSettings {
            Arguments = $"--archive --compress --verbose --progress --recursive {source} {destination}"
        });
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);