Atlas
=====

Atlas is a .NET application framework which allows .NET developers to easily write, run, debug, and deploy Windows Services using a robust configuration model.

Atlas can be configured fluently, or through the app.config. Please refer to the Documentation for more advanced configuration options.

**Use Nuget? -** Atlas is also available via [NuGet](http://nuget.org/List/Packages/Atlas). It is recommended to use NuGet to get Atlas to ease your upgrade path as new builds are released.

####PM&gt; Install-Package Atlas

###How do I use Atlas?###
Simply implement the interface IAmAHostedProcess

<pre><span style="color: blue;">public class</span> MyService : IAmAHostedProcess
{
    <span style="color: blue;">public void</span> Start()
    {
        <span style="color: darkgreen;">// start processing</span>
    }

    <span style="color: blue;">public void</span> Stop()
    {
        <span style="color: darkgreen;">// stop processing</span>
    }

    <span style="color: blue;">public void</span> Pause()
    {
        <span style="color: darkgreen;">// pause processing</span>
    }

    <span style="color: blue;">public void</span> Resume()
    {
        <span style="color: darkgreen;">// resume processing</span>
    }

}</pre>

Then configure the Host and start the configuration in your Main() method. Refer to the <a href="http://atlas.codeplex.com/documentation">Documentation</a> for more configuration options.</p>

<pre><span style="color: blue;">static void</span> Main(<span style="color: blue;">string</span>[] args)
{
    <span style="color: blue;">var</span> configuration = <span style="color: darkcyan;">Host</span>.Configure&lt;<span style="color: darkcyan;">MyService</span>&gt;().WithArguments(args); <span style="color: darkgreen;">// creates configuration with defaults</span>

    <span style="color: darkgreen;">// then just start the configuration and away you go</span>
    <span style="color: darkcyan;">Host</span>.Start(configuration);
}</pre>

Last, simply run your .exe from the command line with the arguments you desire. Refer to the <a href="http://atlas.codeplex.com/documentation">Documentation</a> for arguments list.
<div style="background: black;"><span style="line-height: 34px; font-family: courier new,courier,monospace; color: white; margin-left: 10px;">C:\myservice.exe /console</span></div>
