using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
	[SerializeField] private InputReaderSO _inputReader = default;
	[SerializeField] private GameObject _pauseBook;
	[SerializeField] private GameObject _mainPage;
	[SerializeField] private List<GameObject> _compendiumPages;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		_inputReader.EnableGameplayInput();
		_pauseBook.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnEnable()
	{
		_inputReader.PauseEvent += OnPause;
	}

	private void OnDisable()
	{
		_inputReader.PauseEvent -= OnPause;
	}

	private void disableAllCompendiumPages() {
		foreach(GameObject go in _compendiumPages)
		{
			go.SetActive(false);
		}	
	}
	private void OnPause(InputAction.CallbackContext context)
	{
		_pauseBook.SetActive(!_pauseBook.activeInHierarchy);
		if(_pauseBook.activeInHierarchy)
		{
			_mainPage.SetActive(true);
			disableAllCompendiumPages();
		}
	}

	public void openCompendiumPage(int index)
	{
		_mainPage.SetActive(false);
		_compendiumPages[index].SetActive(true);
	}
}
