﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

    [HeaderAttribute("Nothing selected")]
    public Text selectedProvinceTextObject;

    [HeaderAttribute("Selection Panel")]
    public Image selectionPanel;
    private CanvasGroup selectionCanvasGroup;
    public GameObject selectedProvinceGameObject = null;

    [HeaderAttribute("Information Panel")]
    public Image provinceInformationPanel;
    private CanvasGroup provinceInformationCanvasGroup;
    public Text Name;
    public Text Capital;
    public Text Type;
    public Text Population;
    public Text Income;
    public Text Production;
    public Text CurrentNeed;
    public Text currentStatus;

    [HeaderAttribute("Variables")]
    [RangeAttribute(0.01f, 0.025f)]
    public float typeDelay = 0.075f;

    [HideInInspector] public bool hasSelectedProvince = false;

    private Province selectedProvince;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {

        provinceInformationCanvasGroup = provinceInformationPanel.GetComponent<CanvasGroup> ();
        selectionCanvasGroup = selectionPanel.GetComponent<CanvasGroup> ();

    }

    // Use this for initialization
    void Start() {

        selectionPanel.gameObject.SetActive(true);
        provinceInformationPanel.gameObject.SetActive(true);

        HideSelectionPanel();
        HideProvinceInformation();

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetCurrentSelectedProvince(Province p, bool hasSelectedProvince) {

        if (hasSelectedProvince) {

            this.hasSelectedProvince = true;
            selectedProvince = p;

        } else {

            this.hasSelectedProvince = false;
            selectedProvince = null;

            StopAllCoroutines();
            StartCoroutine(AnimateProvinceText(p.name));

        }

    }

    private IEnumerator AnimateProvinceText(string _province) {

        selectedProvinceTextObject.text = "";

        for (int i = 0; i < _province.Length; i++) {

            selectedProvinceTextObject.text += _province[i];
            yield return new WaitForSeconds(typeDelay);

        }

    }

    public void ResetSelectedProvince() {

        if (!hasSelectedProvince) {

            StopAllCoroutines();
            selectedProvinceTextObject.text = "Hover over the provinces";

        }

    }

    public void DisplaySelectionPanel() {

        selectionCanvasGroup.DOFade(1, 0.5f);
        selectionCanvasGroup.blocksRaycasts = true;

    }

    public void HideSelectionPanel() {

        selectionCanvasGroup.alpha = 0;
        selectionCanvasGroup.blocksRaycasts = false;

    }

    public void DisplayProvinceInformation() {

        if (!selectedProvince.gameObject)
            return;

        HideSelectionPanel();

        selectedProvinceTextObject.text = "";

        Name.text = selectedProvince.name;

        Capital.text = "Capital: " + selectedProvince.capital;

        Type.text = "Type: " + selectedProvince.type.ToString();

        Population.text = "Population: " + selectedProvince.population.ToString();

        Income.text = "Income: " + selectedProvince.monthlyIncome.ToString();

        Production.text = "Production: " + selectedProvince.production.ToString();

        CurrentNeed.text = "Needs: : " + selectedProvince.currentNeed.ToString();

        provinceInformationCanvasGroup.DOFade(1, 0.5f);
        provinceInformationCanvasGroup.blocksRaycasts = true;

    }

    public void HideProvinceInformation() {

        StopAllCoroutines();

        if (selectedProvinceGameObject) {
            
            Debug.Log("De-highlighting selected province: " + selectedProvinceGameObject.name);
            selectedProvinceGameObject.GetComponent<ProvinceController> ().Highlight(false, 0);
            selectedProvinceGameObject = null;

        }

        hasSelectedProvince = false;
        selectedProvince = null;

        selectedProvinceTextObject.text = "Hover over the provinces";

        provinceInformationCanvasGroup.DOFade(0, 0.5f);
        provinceInformationCanvasGroup.blocksRaycasts = false;

        EventSystem.current.SetSelectedGameObject(null);

    }

}