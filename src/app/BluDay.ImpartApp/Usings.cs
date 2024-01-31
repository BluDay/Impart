﻿global using BluDay.Common.Domain.Models;
global using BluDay.Common.Domain.ViewModels;
global using BluDay.Common.Extensions;
global using BluDay.ImpartApp.Core.Domain.Models;
global using BluDay.ImpartApp.Core.Domain.ViewModels;
global using BluDay.ImpartApp.UI.Interactions;
global using BluDay.ImpartApp.UI.Navigation;
global using BluDay.ImpartApp.UI.WindowManagement;
global using CommunityToolkit.Mvvm.ComponentModel;
global using System.ComponentModel.DataAnnotations;

global using IViewModelProvider = BluDay.Common.Extensions.DependencyInjection.IImplementationProvider<BluDay.Common.Domain.ViewModels.IViewModel>;

global using ViewModelProvider = BluDay.Common.Extensions.DependencyInjection.ImplementationProvider<BluDay.Common.Domain.ViewModels.IViewModel>;