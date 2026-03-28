using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Resources.Languages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class PunchSearchService(IActivityService activityService, IPunchService punchService) : IPunchSearchService
    {
        /// <summary>
		/// The activities.
		/// </summary>
		public ObservableRangeCollection<string> Activities { get; set; }

        /// <summary>
        /// The component types.
        /// </summary>
        public ObservableRangeCollection<string> ComponentTypes { get; set; }

        /// <summary>
        /// The component tags.
        /// </summary>
        public ObservableRangeCollection<string> ComponentTags { get; set; }


        /// <summary>
        /// The units
        /// </summary>
        public ObservableRangeCollection<string> Units { get; set; }

        /// <summary>
        /// The CommSystem
        /// </summary>
        public ObservableRangeCollection<string> CommSystem { get; set; }


        /// <summary>
        /// Fetches and populates all Activity search values
        /// </summary>
        public async Task<bool> FetchSearchItems()
        {
            Activities = new ObservableRangeCollection<string>();
            ComponentTypes = new ObservableRangeCollection<string>();
            ComponentTags = new ObservableRangeCollection<string>();
            Units = new ObservableRangeCollection<string>();
            CommSystem = new ObservableRangeCollection<string>();

            await Task.Run(async () =>
            {
                await punchService.FetchPunchListAsync();

                var availableUnits = FetchUnits();
                Units = availableUnits;

                var availableCommSystem = FetchCommSystem();
                CommSystem = availableCommSystem;

                var availableActivities = FetchActivities();
                Activities = availableActivities;

                var availableComponents = FetchComponentTypes();
                ComponentTypes = availableComponents;

                var availableComponentTags = FetchComponentTags();
                ComponentTags = availableComponentTags;
            });

            return true;

        }

        /// <summary>
        /// Fetches and populates Activities names to search on
        /// </summary>
        public ObservableRangeCollection<string> FetchActivities()
        {


            IEnumerable<string> availableActivities = Enumerable.Empty<string>();

            ObservableRangeCollection<string> activities = new ObservableRangeCollection<string>();
            var availablePunchActivities = (from activites in activityService.Activities
                                            join punches in punchService
                                             .Punches on activites.PCAId equals punches.PCAId
                                            select (activites)).ToList();

            availableActivities = availablePunchActivities
                .ToList()
                .Where(p => p.Name != null)
                .OrderBy(p => p.Name)
                .Select(p => p.Name)
                .Distinct();


            activities.Add(UserInterface.SearchView_All);
            activities.AddRange(availableActivities, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
            Activities = activities;
            return activities;

        }

        /// <summary>
        /// Fetches and populates Component type names to search on
        /// </summary>
        public ObservableRangeCollection<string> FetchComponentTypes()
        {
            ObservableRangeCollection<string> component = new ObservableRangeCollection<string>();
            IEnumerable<string> availableComponents = Enumerable.Empty<string>();


            availableComponents = punchService
            .Punches
            .ToList()
            .Where(p => p.ComponentType != null)
            .OrderBy(p => p.ComponentType)
            .Select(p => p.ComponentType.ToString())
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
            IEnumerable<string> availableComponentTags = Enumerable.Empty<string>();


            availableComponentTags = punchService
            .Punches
            .ToList()
            .Where(p => p.TagId != null)
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
        public ObservableRangeCollection<string> FetchUnits()
        {
            ObservableRangeCollection<string> unit = new ObservableRangeCollection<string>();
            IEnumerable<string> availableUnits = Enumerable.Empty<string>();


            try
            {

                availableUnits = punchService
               .Punches
               .ToList()
               .Where(p => p.Unit != null)
               .Select(p => p.Unit)
               .Distinct();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            unit.Add(UserInterface.SearchView_All);

            if (availableUnits != null)
            {
                unit.AddRange(availableUnits, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
                Units = unit;
            }
            return unit;
        }

        /// <summary>
        /// Fetches the comm system.
        /// </summary>
        public ObservableRangeCollection<string> FetchCommSystem()
        {
            ObservableRangeCollection<string> commSystem = new ObservableRangeCollection<string>();
            IEnumerable<string> availableCommSystem = Enumerable.Empty<string>();


            availableCommSystem = punchService
            .Punches
            .ToList()
            .Where(p => p.CommissioningSystem != null)
            .Select(p => p.CommissioningSystem)
            .Distinct();


            commSystem.Add(UserInterface.SearchView_All);
            commSystem.AddRange(availableCommSystem, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
            CommSystem = commSystem;
            return commSystem;

        }

        /// <summary>
        /// Fetches the comm system by unit.
        /// </summary>
        /// <param name="selectedUnit">The selected unit.</param>
        public ObservableRangeCollection<string> FetchCommSystemByUnit(string selectedUnit)
        {

            ObservableRangeCollection<string> commSystem = new ObservableRangeCollection<string>();

            var availableCommSystem = punchService
            .Punches
            .Where(p => p.Unit == selectedUnit && p.CommissioningSystem != null)
            .ToList()
            .Select(p => p.CommissioningSystem)
            .Distinct();

            commSystem.Add(UserInterface.SearchView_All);
            commSystem.AddRange(availableCommSystem, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
            CommSystem = commSystem;
            return commSystem;

        }


        /// <summary>
        /// Fetches the type of the component tags by comp.
        /// </summary>
        /// <param name="selectedCompType">Type of the selected comp.</param>
        public ObservableRangeCollection<string> FetchComponentTagsByCompType(string selectedCompType)
        {

            ObservableRangeCollection<string> componentTags = new ObservableRangeCollection<string>();

            var availableComponentTags = punchService
                .Punches
                .Where(p => p.ComponentType == selectedCompType && p.TagId != null)
                .ToList()
                .Select(p => p.TagId)
                .Distinct();


            componentTags.Add(UserInterface.SearchView_All);
            componentTags.AddRange(availableComponentTags, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
            ComponentTags = componentTags;
            return componentTags;

        }



        /// <summary>
        /// Fetches the component types by comm system.
        /// </summary>
        /// <param name="selectedCommSystem">The selected comm system.</param>
        public ObservableRangeCollection<string> FetchComponentTypesByCommSystem(string selectedCommSystem)
        {

            ObservableRangeCollection<string> component = new ObservableRangeCollection<string>();
            var availableComponents = punchService
                .Punches
                .Where(p => p.CommissioningSystem == selectedCommSystem && p.ComponentType != null)
                .ToList()
                .OrderBy(p => p.ComponentType)
                .Select(p => p.ComponentType)
                .Distinct();

            component.Add(UserInterface.SearchView_All);
            component.AddRange(availableComponents, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
            ComponentTypes = component;

            return component;

        }

        /// <summary>
        /// Fetches the activities by component tag.
        /// </summary>
        /// <param name="selectedComponentTag">The selected component tag.</param>
        public ObservableRangeCollection<string> FetchActivitiesByComponentTag(string selectedComponentTag)
        {

            ObservableRangeCollection<string> activities = new ObservableRangeCollection<string>();
            var availablePunchActivities = from activites in activityService.Activities
                                           join punches in punchService
                                            .Punches on activites.PCAId equals punches.PCAId
                                           select (activites);

            var filteredActivities = availablePunchActivities
                .ToList()
                .Where(p => p.TagId == selectedComponentTag && p.Name != null)
                .OrderBy(p => p.Name)
                .Select(p => p.Name)
                .Distinct();


            activities.Add(UserInterface.SearchView_All);
            activities.AddRange(filteredActivities, System.Collections.Specialized.NotifyCollectionChangedAction.Add);
            Activities = activities;
            return activities;

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
    }
}
