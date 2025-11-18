using Project.Dev.Infrastructure.Registers.Hero;
using UnityEngine;
using Zenject;

namespace Project.Dev.GamePlay.Items
{
    public class CollisionAttach : MonoBehaviour
    {
        [SerializeField] private string requiredSelfTag;
        [SerializeField] private string targetTag;
        [SerializeField] private float maxAttachDistance;
        [SerializeField] private Transform attachPoint;
        [SerializeField] private bool disableAfterAttach;

        private string newLayer = "Default";
        private bool _attached;
        private HeroRegistry _heroRegistry;

        [Inject]
        private void Construct(HeroRegistry heroRegistry)
        {
            _heroRegistry = heroRegistry;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!CompareTag(requiredSelfTag))
                return;

            if (_attached || !other.gameObject.CompareTag(targetTag))
                return;

            if (attachPoint == null)
                return;

            float distance = Vector3.Distance(attachPoint.position, transform.position);
            if (distance > maxAttachDistance)
                return;

            AttachObject(other.gameObject);
        }

        private void AttachObject(GameObject attachObject)
        {
            _attached = true;

            var joint = attachObject.GetComponent<ConfigurableJoint>();
            if (joint != null)
            {
                Destroy(joint); 
            }
            if (attachObject.TryGetComponent<Rigidbody>(out var rb))
            {
                Destroy(rb);
            }

            attachObject.transform.SetParent(transform, true);
            attachObject.transform.position = attachPoint.position;
            attachObject.transform.rotation = attachPoint.rotation;
            attachObject.layer = LayerMask.NameToLayer(newLayer);
            _heroRegistry.HeroInteraction.DropObjectFlag = true;

            if (disableAfterAttach)
                Destroy(this, 0.5f);
        }
    }
}
