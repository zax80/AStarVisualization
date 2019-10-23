﻿using AStarVisualization.Core;
using AStarVisualization.Core.Map;
using AStarVisualization.WPF.Commands;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace AStarVisualization.WPF.ViewModels
{
    public class MapCanvasViewModel : INotifyPropertyChanged
    {
        public Map Map
        {
            get => _map;
            set
            {
                if (_map != value)
                {
                    _map = value;
                    _map.UpdateNodeIndices();
                    OnPropertyChanged("Map");
                }
            }
        }
        public List<Node> Path
        {
            get => _path;
            set
            {
                if (_path != value)
                {
                    _path = value;
                    OnPropertyChanged("Path");
                }
            }
        }
        public ICommand PlaceTileCommand => _placeTileCommand;
        public ICommand HandleLeftClickCommand => _handleLeftClickCommand;
        public ICommand RemoveTileCommand => _removeTileCommand;

        // TODO: make sure that these properties are used correctly
        public Place Place { get; set; } = Place.None;
        public bool MapDesignPhaseActive { get; set; } = true;

        private Map _map;
        private List<Node> _path;

        private readonly ICommand _placeTileCommand;
        private readonly ICommand _handleLeftClickCommand;
        private readonly ICommand _removeTileCommand;

        public MapCanvasViewModel()
        {
            _map = new Map(0, 0);
            _path = new List<Node>();

            // TODO: maybe inject these with Ninject
            _placeTileCommand = new PlaceTileCommand(this);
            _handleLeftClickCommand = new HandleLeftClickCommand(this);
            _removeTileCommand = new RemoveTileCommand(this);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
