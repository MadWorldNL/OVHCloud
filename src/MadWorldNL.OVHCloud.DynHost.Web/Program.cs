using MadWorldNL.OVHCloud.DynHost.Lib;
using MadWorldNL.OVHCloud.DynHost.Lib.Contracts;
using MadWorldNL.OVHCloud.DynHost.Lib.Domain;
using MadWorldNL.OVHCloud.DynHost.Web.Endpoints;
using MadWorldNL.OVHCloud.DynHost.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.Configure<DynHostSettings>(
    builder.Configuration.GetSection(DynHostSettings.Key));
builder.Services.Configure<StorageSettings>(
    builder.Configuration.GetSection(StorageSettings.Key));

builder.Services.AddSingleton<IIpdAddressLocator, IpAddressLocator>();
builder.Services.AddSingleton<IDynHost, DynHost>();
builder.Services.AddSingleton<IIpAddressStorage, IpAddressStorage>();
builder.Services.AddSingleton<IOVHCloudClient, OVHCloudClient>();

builder.Services.AddHostedService<RegulatorIpAddressUpdaterService>();

var app = builder.Build();

app.AddDynHostEndpoints();

app.Run();