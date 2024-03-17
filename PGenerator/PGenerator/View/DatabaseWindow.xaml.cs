﻿using System.Windows;
using PGenerator.Service.InformationService;
using PGenerator.ViewModel;

namespace PGenerator.View;

public partial class DatabaseWindow : Window
{
    public DatabaseWindow() { }
    public DatabaseWindow(IInformationService informationService, string userId)
    {
        InitializeComponent();
        DataContext = new DatabaseViewModel(informationService, userId);
    }
}