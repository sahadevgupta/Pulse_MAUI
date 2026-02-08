using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Resources.Languages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Services
{

    public class ActivitySearchService : IActivitySearchService
    {
        /// <summary>
        /// The activities.
        /// </summary>
        public ObservableRangeCollection<Activity>? Activities { get; set; }

        /// <summary>
        /// The activities.
        /// </summary>
        public ObservableRangeCollection<string>? ActivitiesName { get; set; }

        /// <summary>
        /// The component types.
        /// </summary>
        public ObservableRangeCollection<string>? ComponentTypes { get; set; }

        /// <summary>
        /// The component tags.
        /// </summary>
        public ObservableRangeCollection<string>? ComponentTags { get; set; }

        /// <summary>
        /// The units
        /// </summary>
        public ObservableRangeCollection<string>? Units { get; set; }

        /// <summary>
        /// The comm system
        /// </summary>
        /// 
        public ObservableRangeCollection<string>? CommSystem { get; set; }

        /// <summary>
        /// Fetches and populates all Activity search values
        /// </summary>
        public async Task<bool> FetchSearchItems(IEnumerable<Activity> activities)
        {
            this.Activities = new ObservableRangeCollection<Activity>(activities);
            ActivitiesName = new ObservableRangeCollection<string>();
            ComponentTypes = new ObservableRangeCollection<string>();
            ComponentTags = new ObservableRangeCollection<string>();
            Units = new ObservableRangeCollection<string>();
            CommSystem = new ObservableRangeCollection<string>();

            await Task.Run(() =>
            {
                var availableActivities = FetchActivities();
                ActivitiesName = availableActivities;

                var availableComponents = FetchComponentTypes();
                ComponentTypes = availableComponents;

                var availableComponentTags = FetchComponentTags();
                ComponentTags = availableComponentTags;

                var availableUnits = FetchUnits();
                Units = availableUnits;

                var availableCommSystem = FetchCommSystem();
                CommSystem = availableCommSystem;

            });
            return true;
        }

        /// <summary>
        /// Fetches the comm system by unit.
        /// </summary>
        /// <param name="selectedUnit">The selected unit.</param>
        public ObservableRangeCollection<string> FetchCommSystemByUnit(string selectedUnit)
        {
            ObservableRangeCollection<string> commSystem = new ObservableRangeCollection<string>();

            var availableCommSystem = Activities
                .Where(p => p.Unit == selectedUnit)
                .Select(p => p.CommissioningSystem)
                .Distinct();

            commSystem.Add(UserInterface.SearchView_All);
            commSystem.AddRange(availableCommSystem, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
            CommSystem = commSystem;
            return commSystem;

        }

        /// <summary>
        /// Checks if an options is selected
        /// </summary>
        /// <returns><c>true</c>, if option selected, <c>false</c> otherwise.</returns>
        /// <param name="option">Option.</param>
        public static bool HasOptionSelected(string option)
        {
            if (!string.IsNullOrEmpty(option) && option != UserInterface.SearchView_All)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Fetches and populates Activities names to search on
        /// </summary>
        public ObservableRangeCollection<string> FetchActivities()
        {

            ObservableRangeCollection<string> activities = new ObservableRangeCollection<string>();

            var availableActivities = Activities.OrderBy(p => p.Name)
                                                .Select(p => p.Name)
                                                .Distinct();

            activities.Add(UserInterface.SearchView_All);
            activities.AddRange(availableActivities, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
            ActivitiesName = activities;
            return activities;
        }

        /// <summary>
        /// Fetches and populates Component type names to search on
        /// </summary>
        public ObservableRangeCollection<string> FetchComponentTypes()
        {
            ObservableRangeCollection<string> component = new ObservableRangeCollection<string>();
            var availableComponents = Activities.OrderBy(p => p.ComponentType)
                                                .Select(p => p.ComponentType)
                                                .Distinct();

            component.Add(UserInterface.SearchView_All);
            component.AddRange(availableComponents, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
            ComponentTypes = component;
            return component;
        }

        /// <summary>
        /// Fetches the component types by comm system.
        /// </summary>
        /// <param name="selectedCommSystem">The selected comm system.</param>
        public ObservableRangeCollection<string> FetchComponentTypesByCommSystem(string selectedCommSystem)
        {
            ObservableRangeCollection<string> component = new ObservableRangeCollection<string>();
            var availableComponents = Activities
                .Where(p => p.CommissioningSystem == selectedCommSystem)
                .OrderBy(p => p.ComponentType)
                .Select(p => p.ComponentType)
                .Distinct();

            component.Add(UserInterface.SearchView_All);
            component.AddRange(availableComponents, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
            ComponentTypes = component;
            return component;

        }


        /// <summary>
        /// Fetches and populates Component Tags to search on
        /// </summary>
        public ObservableRangeCollection<string> FetchComponentTags()
        {

            ObservableRangeCollection<string> componentTags = new ObservableRangeCollection<string>();

            var availableComponentTags = Activities
                .Select(p => p.TagId)
                .Distinct();

            componentTags.Add(UserInterface.SearchView_All);
            componentTags.AddRange(availableComponentTags, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
            ComponentTags = componentTags;
            return componentTags;

        }

        /// <summary>
        /// Fetches the type of the component tags by comp.
        /// </summary>
        /// <param name="selectedCompType">Type of the selected comp.</param>
        public ObservableRangeCollection<string> FetchComponentTagsByCompType(string selectedCompType)
        {
            ObservableRangeCollection<string> componentTags = new ObservableRangeCollection<string>();

            var availableComponentTags = Activities
                .Where(p => p.ComponentType == selectedCompType)
                .Select(p => p.TagId)
                .Distinct();

            componentTags.Add(UserInterface.SearchView_All);
            componentTags.AddRange(availableComponentTags, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
            ComponentTags = componentTags;
            return componentTags;
        }


        /// <summary>
        /// Fetches the units.
        /// </summary>
        private ObservableRangeCollection<string> FetchUnits()
        {

            ObservableRangeCollection<string> unit = new ObservableRangeCollection<string>();
            var availableUnits = Activities
                .Select(p => p.Unit)
                .Distinct();

            unit.Add(UserInterface.SearchView_All);
            unit.AddRange(availableUnits, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
            return unit;

        }

        /// <summary>
        /// Fetches the comm system.
        /// </summary>
        public ObservableRangeCollection<string> FetchCommSystem()
        {
            ObservableRangeCollection<string> commSystem = new ObservableRangeCollection<string>();

            var availableCommSystem = Activities
                .Select(p => p.CommissioningSystem)
                .Distinct();

            commSystem.Add(UserInterface.SearchView_All);
            commSystem.AddRange(availableCommSystem, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
            CommSystem = commSystem;
            return commSystem;

        }

        /// <summary>
        /// Fetches the activities by component tag.
        /// </summary>
        /// <param name="selectedComponentTag">The selected component tag.</param>
        public ObservableRangeCollection<string> FetchActivitiesByComponentTag(string selectedComponentTag)
        {
            ObservableRangeCollection<string> activities = new ObservableRangeCollection<string>();

            var availableActivities = Activities
                .Where(p => p.TagId == selectedComponentTag)
                .ToList()
                .OrderBy(p => p.Name)
                .Select(p => p.Name)
                .Distinct();

            activities.Add(UserInterface.SearchView_All);
            activities.AddRange(availableActivities, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
            ActivitiesName = activities;
            return activities;
        }

    }

    //   /// <summary>
    ///// Class for handling Activity search functionality
    ///// Fogbugz Case:
    ///// Author: Manuel Dambrine
    ///// Created: 29/03/2013
    ///// </summary>
    //public class ActivitySearchService
    //   {

    //       public int DataLoaded;
    //       public bool UIInitalised;

    //       private static ActivitySearchService instance;

    //       /// <summary>
    //       /// Gets the instance.
    //       /// </summary>
    //       /// <value>The instance.</value>
    //       public static ActivitySearchService Instance
    //       {
    //           get
    //           {
    //               if (instance == null)
    //               {
    //                   instance = new ActivitySearchService();
    //               }

    //               return instance;
    //           }
    //       }

    //       /// <summary>
    //       /// The activities.
    //       /// </summary>
    //       public ObservableRangeCollection<string> Activities;

    //       /// <summary>
    //       /// The component types.
    //       /// </summary>
    //       public ObservableRangeCollection<string> ComponentTypes;

    //       /// <summary>
    //       /// The component tags.
    //       /// </summary>
    //       public ObservableRangeCollection<string> ComponentTags;


    //       /// <summary>
    //       /// The units
    //       /// </summary>
    //       public ObservableRangeCollection<string> Units;


    //       /// <summary>
    //       /// The comm system
    //       /// </summary>
    //       /// 
    //       public ObservableRangeCollection<string> CommSystem;
    //       /// <summary>
    //       /// Gets or sets the selected activity.
    //       /// </summary>
    //       /// <value>The selected activity.</value>
    //       public string SelectedActivity { get; set; }

    //       /// <summary>
    //       /// Gets or sets the selected component tag.
    //       /// </summary>
    //       /// <value>The selected component tag.</value>
    //       public string SelectedComponentTag { get; set; }

    //       /// <summary>
    //       /// Gets or sets the type of the selected component.
    //       /// </summary>
    //       /// <value>The type of the selected component.</value>
    //       public string SelectedComponentType { get; set; }


    //       /// <summary>
    //       /// Gets or sets the selected unit.
    //       /// </summary>
    //       /// <value>
    //       /// The selected unit.
    //       /// </value>
    //       public string SelectedUnit { get; set; }

    //       /// <summary>
    //       /// Gets or sets the selected comm system.
    //       /// </summary>
    //       /// <value>
    //       /// The selected comm system.
    //       /// </value>
    //       public string SelectedCommSystem { get; set; }

    //       /// <summary>
    //       /// Initializes a new instance of the <see cref="T:PCATablet.Core.Services.ActivitySearchService"/> class.
    //       /// </summary>
    //       private ActivitySearchService()
    //       {
    //           Activities = new ObservableRangeCollection<string>();
    //           ComponentTypes = new ObservableRangeCollection<string>();
    //           ComponentTags = new ObservableRangeCollection<string>();
    //           Units = new ObservableRangeCollection<string>();
    //           CommSystem = new ObservableRangeCollection<string>();

    //       }


    //       /// <summary>
    //       /// Fetches and populates all Activity search values
    //       /// </summary>
    //       public async Task<bool> FetchSearchItems()
    //       {

    //           Activities = new ObservableRangeCollection<string>();
    //           ComponentTypes = new ObservableRangeCollection<string>();
    //           ComponentTags = new ObservableRangeCollection<string>();
    //           Units = new ObservableRangeCollection<string>();
    //           CommSystem = new ObservableRangeCollection<string>();

    //           await ActivityService.Instance.FetchActivityListAsync();

    //           await Task.Run(() =>
    //           {
    //               var availableActivities = FetchActivities();
    //               Activities = availableActivities;

    //               var availableComponents = FetchComponentTypes();
    //               ComponentTypes = availableComponents;

    //               var availableComponentTags = FetchComponentTags();
    //               ComponentTags = availableComponentTags;

    //               var availableUnits = FetchUnits();
    //               Units = availableUnits;

    //               var availableCommSystem = FetchCommSystem();
    //               CommSystem = availableCommSystem;

    //           });

    //           DataLoaded = DataLoaded + 1;
    //           return true;
    //       }








}
