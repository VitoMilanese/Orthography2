using System;
using System.Collections.ObjectModel;
using System.Linq;
using OrthographyMobile.Models;
using OrthographyMobile.Models.dict;
using OrthographyMobile.Models.enums;

namespace OrthographyMobile.ViewModels.Helpers
{
	public class GeneratedPackageToken : BaseViewModel
	{
		private GeneratedPackage m_package;
		public GeneratedPackage Package
		{
			get => m_package;
			set
			{
				if (value == null) throw new Exception("Package must be initialized");
				IsBusy = true;
				SetProperty(ref m_package, value);

				Relation = m_package.Relation;
				Word = m_package.Word;
				Rule = m_package.Rule;

				Mode = Data.Modes.FirstOrDefault(p => p.ID == Rule.ModeID);

				Data.Numbers.Clear();
				AvailableNumbers = m_package.AvailableNumbers;
				CopyItems(AvailableNumbers, Data.Numbers);
				Number = Data.Numbers.FirstOrDefault(p => p.ID == Rule.NumberID);

				Data.Persons.Clear();
				AvailablePersons = m_package.AvailablePersons;
				CopyItems(AvailablePersons, Data.Persons);
				Person = Data.Persons.FirstOrDefault(p => p.ID == Rule.PersonID);

				Data.Genders.Clear();
				AvailableGenders = m_package.AvailableGenders;
				CopyItems(AvailableGenders, Data.Genders);
				Gender = Data.Genders.FirstOrDefault(p => p.ID == Rule.GenderID);
				IsBusy = false;
			}
		}

		#region Properties
		private int m_selectedID;
		public int SelectedID
		{
			get => m_selectedID;
			set => SetProperty(ref m_selectedID, value);
		}

		private Relation m_relation = new Relation();
		public Relation Relation
		{
			get => m_relation;
			set => SetProperty(ref m_relation, value);
		}

		private Word m_word = new Word();
		public Word Word
		{
			get => m_word;
			set => SetProperty(ref m_word, value);
		}

		private Rule m_rule = new Rule();
		public Rule Rule
		{
			get => m_rule;
			set => SetProperty(ref m_rule, value);
		}

		public int PreviousMode { get; private set; }
		private Mode m_mode;
		public Mode Mode
		{
			get => m_mode;
			set
			{
				if (value != null)
				{
					PreviousMode = m_mode?.ID ?? 0;
					SetProperty(ref m_mode, value);
				}
			}
		}

		public int PreviousNumber { get; private set; }
		private Number m_number;
		public Number Number
		{
			get => m_number;
			set
			{
				if (value != null)
				{
					PreviousNumber = m_number?.ID ?? 0;
					SetProperty(ref m_number, value);
				}
			}
		}

		public int PreviousPerson { get; private set; }
		private Person m_person;
		public Person Person
		{
			get => m_person;
			set
			{
				if (value != null)
				{
					PreviousPerson = m_person?.ID ?? 0;
					SetProperty(ref m_person, value);
				}
			}
		}

		public int PreviousGender { get; private set; }
		private Gender m_gender;
		public Gender Gender
		{
			get => m_gender;
			set
			{
				if (value != null)
				{
					PreviousGender = m_gender?.ID ?? 0;
					SetProperty(ref m_gender, value);
				}
			}
		}

		private Number[] m_availableNumbers;
		public Number[] AvailableNumbers
		{
			get => m_availableNumbers;
			set => SetProperty(ref m_availableNumbers, value);
		}

		private Person[] m_availablePersons;
		public Person[] AvailablePersons
		{
			get => m_availablePersons;
			set => SetProperty(ref m_availablePersons, value);
		}

		private Gender[] m_vailableGenders;
		public Gender[] AvailableGenders
		{
			get => m_vailableGenders;
			set => SetProperty(ref m_vailableGenders, value);
		}
		#endregion

		private void CopyItems<T>(T[] from, Collection<T> to) where T : IdValue
		{
			to.Clear();
			foreach (var item in from)
			{
				item.Label = Data.Labels?.FirstOrDefault(p => p.ID == item.LabelID)?.Value ?? "no label";
				to.Add(item);
			}
		}
	}
}
