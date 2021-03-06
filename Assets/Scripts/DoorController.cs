using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public sealed class DoorController : MonoBehaviour {

	public enum DoorState {
		Close,
		Open,
		Closing,
		Opening
	}

	[Header("Setup")]
	public Transform door;
	public DoorState initialState;
	public bool oneTime;
	public float transitionTime = 2f;
	public Vector3 openShift = new Vector3(.0f, -1.0f, .0f);

	[Header("Runtime")]
	[SerializeField]
	private int _openedTimes;
	[SerializeField]
	private DoorState _state;
	[SerializeField]
	private bool _openBuffer;
	[SerializeField]
	private bool _closeBuffer;
	[SerializeField]
	private Vector3 _openPosition;
	[SerializeField]
	private Vector3 _closePosition;
	[SerializeField]
	private CoroutineTask _operationTask;

	private void Awake() {
		_operationTask = new CoroutineTask(this);
		_state = initialState;
		_closePosition = door.localPosition;
		_openPosition = _closePosition + openShift;
		
		if (initialState == DoorState.Open) door.localPosition = _openPosition;
	}

	public void Open() {
		if (oneTime && _openedTimes > 0) return;
		if (_state == DoorState.Closing) _openBuffer = true;
		if (_state != DoorState.Close) return; 
		// Debug.Log("Door Open");
		_state = DoorState.Opening;
		_openedTimes += 1;
		_operationTask.StartCoroutine(ExeOperationTask(_closePosition, _openPosition, DoorState.Open));
	}

	public void Close() {
		if (oneTime) return;
		if (_state == DoorState.Opening) _closeBuffer = true;
		if (_state != DoorState.Open) return; 
		// Debug.Log("Door Close");
		_state = DoorState.Closing;
		_operationTask.StartCoroutine(ExeOperationTask(_openPosition, _closePosition, DoorState.Close));
	}

	private IEnumerator ExeOperationTask(Vector3 initialPosition, Vector3 targetPosition, DoorState endState) {
		float progress = .0f;	
		float initialTime = Time.time;
		while (progress < 1.0f) {
			yield return CoroutineTask.WaitForNextFrame;
			progress = Mathf.Clamp01((Time.time - initialTime) / transitionTime);
			door.localPosition = Vector3.Lerp(initialPosition, targetPosition, progress);
		}

		door.localPosition = targetPosition;
		_state = endState;
		
		// if (oneTime && endState == DoorState.Close) gameObject.SetActive(false);

		if (_state == DoorState.Close && _openBuffer) {
			_openBuffer = false;
			Open();
		} else if (_state == DoorState.Open && _closeBuffer) {
			_closeBuffer = false;
			Close();
		}
	}

	private void OnDestroy() {
		if (_operationTask != null) {
			_operationTask.StopCoroutine();
			_operationTask = null;
		}
	}
}