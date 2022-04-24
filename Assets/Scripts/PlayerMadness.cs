using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMadness : MonoBehaviour {

    public int maxMadness;
    private int currMadness;
    public int recovRate;
    public int recovCooldown;
    private bool canRecover = true;
    private CoroutineTask cooldownTask;
    private CoroutineTask recovMadnessTask;

    [SerializeField]
    private int _Madness;
    [SerializeField]
    private int _recovRate;
    [SerializeField]
    private int _recovCooldown;

    public int CurrentMadness => _Madness;

    // Start is called before the first frame update
    void Start()
    {
        _Madness = maxMadness;
        _recovRate = recovRate;
        _recovCooldown = recovCooldown;
        cooldownTask = new CoroutineTask(this);
        recovMadnessTask = new CoroutineTask(this);
        recovMadnessTask.StartCoroutine(recovMadness());
    }

    public void gainMadness (int amount) {
        _Madness = Mathf.Min(_Madness + amount, maxMadness);
        canRecover = true;
    }

    public int consumeMadness (int amount, bool partial = false) {
        int initMadness = _Madness;
        if (partial) {
            _Madness = Mathf.Max(_Madness - amount, 0);
            if (initMadness != 0 && _Madness == 0) {
                cooldownTask.StartCoroutine(calcCooldown());
            }
            return Mathf.Max(amount, amount - initMadness);
        } else {
            if (_Madness - amount > 0) {
                _Madness -= amount;
                return amount;
            } else if (_Madness - amount == 0) {
                _Madness -= amount;
                cooldownTask.StartCoroutine(calcCooldown());
                return amount;
            } else {
                return 0;
            }
        }
    }



    IEnumerator recovMadness() {
        while (true) {
            if (canRecover) {
                gainMadness(_recovRate);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator calcCooldown() {
        canRecover = false;
        yield return new WaitForSeconds(_recovCooldown);
        canRecover = true;
    }
}
