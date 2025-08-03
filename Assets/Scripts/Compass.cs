using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Compass : MonoBehaviour
{
    public Transform _playerLocation;
    public Transform _farmLocation;

    private RectTransform _needleTransform;
    private Vector2 directionVector;
	private void Start()
	{
        _playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        _farmLocation = GameObject.FindGameObjectWithTag("Farm").transform;
        _needleTransform = GetComponent<RectTransform>();
	}
	// Update is called once per frame
	void Update()
    {
        directionVector = (_farmLocation.position - _playerLocation.position).normalized;
        float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
        angle -= 90f;
        _needleTransform.eulerAngles = new Vector3(0, 0, angle);
    }

}
