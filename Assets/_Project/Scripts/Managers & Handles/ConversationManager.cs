using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

namespace ACoolTeam
{
    public class ConversationManager : MonoBehaviour
    {
        public static ConversationManager Instance { get; private set; }

        [SerializeField] private TextMeshProUGUI _dialogueText;
        [SerializeField] private TextMeshProUGUI _characterName;
        [SerializeField] private Image _displayPic;
        [SerializeField] private Image _dialogueBox;
        [SerializeField] private GameObject _dialogueDisplay;
        [SerializeField] private AudioClip _textSound;
        [SerializeField] private int _charLimit;

        private bool _isTalking = false;
        private PlayerInput _playerInput;
        private bool _enterPressed;
        private GameObject _conversationStarter;
        private bool _isTyping = false;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else if (Instance != this) Destroy(this);

            _dialogueDisplay.SetActive(false);

            _playerInput = new PlayerInput();

            _playerInput.Player.Enter.started += NextEntry;
            _playerInput.Player.Enter.canceled += NextEntry;
        }

        private void NextEntry(InputAction.CallbackContext context)
        {
            _enterPressed = context.ReadValue<float>() > 0;
        }

        private void OnEnable()
        {
            _playerInput.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Disable();
        }

        public void StartConversation(ConversationObject currentConversation, bool isTriggered, GameObject talker)
        {
            if (!_isTalking)
            {
                _isTalking = true;
                _dialogueDisplay.SetActive(true);

                StartCoroutine(ConversationCo(currentConversation, isTriggered, talker));
            }
        }

        private IEnumerator ConversationCo(ConversationObject currentConversation, bool isTriggered, GameObject talker)
        {
            foreach (ConversationEntryObject entry in currentConversation.ConversationLines)
            {
                PopulateCurrentEntry(entry);
                yield return WaitForPlayerCo();
            }

            _dialogueDisplay.SetActive(false);
            _isTalking = false;
            if (isTriggered)
            {
                talker.SetActive(false);
            }
        }

        private void PopulateCurrentEntry(ConversationEntryObject entry)
        {
            _displayPic.sprite = entry.DisplayPic;
            _characterName.text = entry.CharName;
            StartCoroutine(ScrollText(entry.Lines));
        }

        private IEnumerator WaitForPlayerCo()
        {
            SoundManager.Instance.PlaySFX(_textSound, 0.2f);
            yield return new WaitForSeconds(0.5f);
            while (!_enterPressed || _isTyping) //there was a glitch where if you pressed enter too soon, it'd skip ahead and intertwine both lines of text LOL. This fixes it.
            {
                yield return null;
            }
        }

        private IEnumerator ScrollText(string text)
        {
            _isTyping = true;
            _dialogueText.text = String.Empty;
            if (text.Length > _charLimit)
            {
                text = text.Substring(0, _charLimit); //if the text goes over, it just gets cut off.
            }
            foreach (char chr in text)
            {
                _dialogueText.text += chr;
                yield return new WaitForSeconds(0.025f);
            }
            _isTyping = false;
        }

        public bool IsTalking()
        {
            return _isTalking;
        }
    }
}
