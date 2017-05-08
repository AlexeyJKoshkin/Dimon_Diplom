using ShutEye.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShutEye.Core
{
    public class SpagettiRecipe
    {
        public bool Cooking
        {
            get { return _actionsQueue.Count > 0; }
        }

        public IEnumerator Current
        {
            get { return _actionsQueue.Dequeue(); }
        }

        public string Name { get; private set; }
        public int Count { get { return _actionsQueue.Count; } }
        private readonly Queue<IEnumerator> _actionsQueue;

        public System.Action callback;

        public SpagettiRecipe(IEnumerable<IEnumerator> actions, string name = "No name", System.Action calback = null)
        {
            Name = name;
            _actionsQueue = new Queue<IEnumerator>(actions);
            this.callback = calback;
        }

        public SpagettiRecipe(string name = "No name", System.Action calback = null)
        {
            Name = name;
            _actionsQueue = new Queue<IEnumerator>();
            this.callback = calback;
        }

        public SpagettiRecipe Add(IEnumerator action)
        {
            _actionsQueue.Enqueue(action);
            return this;
        }

        public SpagettiRecipe Add(IEnumerable<IEnumerator> actions)
        {
            actions.ForEach(_actionsQueue.Enqueue);
            return this;
        }
    }

    public partial class GameCore
    {
        public void AddSpaggetti(SpagettiRecipe recipe)
        {
            StartCoroutine(Boulig(recipe));
        }

        public IEnumerator WaitAddSpaggetti(SpagettiRecipe recipe)
        {
            yield return StartCoroutine(Boulig(recipe));
        }

        private IEnumerator Boulig(SpagettiRecipe recipe)
        {
            while (recipe.Cooking)
            {
                yield return recipe.Current;
            }
            Debug.Log("[SD] Done " + recipe.Name);
            if (recipe.callback != null)
                recipe.callback.Invoke();
        }
    }
}