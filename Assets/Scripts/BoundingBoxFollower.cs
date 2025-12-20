using System;
using System.Collections.Generic;
using Spine;
using UnityEngine;

[ExecuteInEditMode]
public class BoundingBoxFollower : MonoBehaviour
{
	public string CurrentAttachmentName
	{
		get
		{
			return this.currentAttachmentName;
		}
	}

	public BoundingBoxAttachment CurrentAttachment
	{
		get
		{
			return this.currentAttachment;
		}
	}

	public PolygonCollider2D CurrentCollider
	{
		get
		{
			return this.currentCollider;
		}
	}

	public Slot Slot
	{
		get
		{
			return this.slot;
		}
	}

	private void OnEnable()
	{
		this.ClearColliders();
		if (this.skeletonRenderer == null)
		{
			this.skeletonRenderer = base.GetComponentInParent<SkeletonRenderer>();
		}
		if (this.skeletonRenderer != null)
		{
			SkeletonRenderer skeletonRenderer = this.skeletonRenderer;
			skeletonRenderer.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(skeletonRenderer.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(this.HandleReset));
			SkeletonRenderer skeletonRenderer2 = this.skeletonRenderer;
			skeletonRenderer2.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Combine(skeletonRenderer2.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(this.HandleReset));
		}
	}

	private void OnDisable()
	{
		SkeletonRenderer skeletonRenderer = this.skeletonRenderer;
		skeletonRenderer.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(skeletonRenderer.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(this.HandleReset));
	}

	private void Start()
	{
		if (!this.hasReset && this.skeletonRenderer != null)
		{
			this.HandleReset(this.skeletonRenderer);
		}
	}

	public void HandleReset(SkeletonRenderer renderer)
	{
		if (this.slotName == null || this.slotName == string.Empty)
		{
			return;
		}
		this.hasReset = true;
		this.ClearColliders();
		this.colliderTable.Clear();
		if (this.skeletonRenderer.skeleton == null)
		{
			SkeletonRenderer skeletonRenderer = this.skeletonRenderer;
			skeletonRenderer.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(skeletonRenderer.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(this.HandleReset));
			this.skeletonRenderer.Reset();
			SkeletonRenderer skeletonRenderer2 = this.skeletonRenderer;
			skeletonRenderer2.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Combine(skeletonRenderer2.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(this.HandleReset));
		}
		Skeleton skeleton = this.skeletonRenderer.skeleton;
		this.slot = skeleton.FindSlot(this.slotName);
		int slotIndex = skeleton.FindSlotIndex(this.slotName);
		foreach (Skin skin in skeleton.Data.Skins)
		{
			List<string> list = new List<string>();
			skin.FindNamesForSlot(slotIndex, list);
			foreach (string text in list)
			{
				Attachment attachment = skin.GetAttachment(slotIndex, text);
				if (attachment is BoundingBoxAttachment)
				{
					PolygonCollider2D polygonCollider2D = SkeletonUtility.AddBoundingBoxAsComponent((BoundingBoxAttachment)attachment, base.gameObject, true);
					polygonCollider2D.enabled = false;
					polygonCollider2D.hideFlags = HideFlags.HideInInspector;
					this.colliderTable.Add((BoundingBoxAttachment)attachment, polygonCollider2D);
					this.attachmentNameTable.Add((BoundingBoxAttachment)attachment, text);
				}
			}
		}
		if (this.colliderTable.Count == 0)
		{
			this.valid = false;
		}
		else
		{
			this.valid = true;
		}
		if (!this.valid)
		{
			UnityEngine.Debug.LogWarning("Bounding Box Follower not valid! Slot [" + this.slotName + "] does not contain any Bounding Box Attachments!");
		}
	}

	private void ClearColliders()
	{
		PolygonCollider2D[] components = base.GetComponents<PolygonCollider2D>();
		if (Application.isPlaying)
		{
			foreach (PolygonCollider2D obj in components)
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
		else
		{
			foreach (PolygonCollider2D obj2 in components)
			{
				UnityEngine.Object.DestroyImmediate(obj2);
			}
		}
		this.colliderTable.Clear();
		this.attachmentNameTable.Clear();
	}

	private void LateUpdate()
	{
		if (!this.skeletonRenderer.valid)
		{
			return;
		}
		if (this.slot != null && this.slot.Attachment != this.currentAttachment)
		{
			this.SetCurrent((BoundingBoxAttachment)this.slot.Attachment);
		}
	}

	private void SetCurrent(BoundingBoxAttachment attachment)
	{
		if (this.currentCollider)
		{
			this.currentCollider.enabled = false;
		}
		if (attachment != null)
		{
			this.currentCollider = this.colliderTable[attachment];
			this.currentCollider.enabled = true;
		}
		else
		{
			this.currentCollider = null;
		}
		this.currentAttachment = attachment;
		this.currentAttachmentName = ((this.currentAttachment != null) ? this.attachmentNameTable[attachment] : null);
	}

	public SkeletonRenderer skeletonRenderer;

	[SpineSlot("", "skeletonRenderer")]
	public string slotName;

	[Tooltip("LOL JK, Someone else do it!")]
	public bool use3DMeshCollider;

	private Slot slot;

	private BoundingBoxAttachment currentAttachment;

	private PolygonCollider2D currentCollider;

	private string currentAttachmentName;

	private bool valid;

	private bool hasReset;

	public Dictionary<BoundingBoxAttachment, PolygonCollider2D> colliderTable = new Dictionary<BoundingBoxAttachment, PolygonCollider2D>();

	public Dictionary<BoundingBoxAttachment, string> attachmentNameTable = new Dictionary<BoundingBoxAttachment, string>();
}
