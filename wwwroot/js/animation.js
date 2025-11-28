window.Activity3D = {
  scene: null,
  camera: null,
  renderer: null,
  activities: [],
  cameraTarget: new THREE.Vector3(),
  orbitAngle: 0,
  basePosition: new THREE.Vector3(0, 0, 0),
  clusterRadius: 4, // initial cluster radius
  growthFactor: 0.1, // cluster expands per new cube
  animationRunning: false, // prevent multiple loops

  loadThreeJS: function () {
    if (!window.THREE) {
      const script = document.createElement("script");
      script.src =
        "https://cdn.jsdelivr.net/npm/three@0.158.0/build/three.min.js";
      script.onload = () => console.log("Three.js loaded");
      document.head.appendChild(script);
    }
  },

  init: function (containerId) {
    if (!window.THREE) {
      console.error("Three.js not loaded yet!");
      return;
    }

    const container = document.getElementById(containerId);
    if (!container) {
      console.error("Container not found!");
      return;
    }
    const width = container.clientWidth;
    const height = container.clientHeight;

    this.scene = new THREE.Scene();
    this.camera = new THREE.PerspectiveCamera(60, width / height, 0.1, 2000);

    this.renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
    this.renderer.setSize(width, height);

    container.appendChild(this.renderer.domElement);

    // Add lights
    const sunlight = new THREE.DirectionalLight(0xffffff, 1.2);
    sunlight.position.set(10, 20, 10);
    this.scene.add(sunlight);

    const ambient = new THREE.AmbientLight(0x404040);
    this.scene.add(ambient);

    // Start animation loop once
    if (!this.animationRunning) {
      this.animate();
    }

    // Add window resize event listener
    window.addEventListener("resize", () => this.onWindowResize(containerId));
  },

  onWindowResize: function (containerId) {
    const container = document.getElementById(containerId);
    if (!container || !this.camera || !this.renderer) return;

    const width = container.clientWidth;
    const height = container.clientHeight;

    this.camera.aspect = width / height;
    this.camera.updateProjectionMatrix();

    this.renderer.setSize(width, height);
  },

  addCube: function (size = 1) {
    if (!this.scene) return;

    // Random color
    const color = Math.floor(Math.random() * 0xffffff);

    // Random spherical-ish position around center
    const theta = Math.random() * 2 * Math.PI;
    const phi = Math.random() * Math.PI;
    const r = this.clusterRadius;

    const x = this.basePosition.x + r * Math.sin(phi) * Math.cos(theta);
    const y = this.basePosition.y + r * Math.sin(phi) * Math.sin(theta);
    const z = this.basePosition.z + r * Math.cos(phi);

    this.clusterRadius += this.growthFactor;

    const geometry = new THREE.BoxGeometry(size, size, size);
    const material = new THREE.MeshStandardMaterial({ color });
    const cube = new THREE.Mesh(geometry, material);

    cube.scale.set(0, 0, 0);
    cube.position.set(x, y, z);

    this.scene.add(cube);
    this.activities.push({ mesh: cube, targetScale: size });
  },

  animate: function () {
    this.animationRunning = true;
    requestAnimationFrame(this._animateLoop.bind(this));
  },

  _animateLoop: function () {
    requestAnimationFrame(this._animateLoop.bind(this));

    // Animate cube pop-in
    this.activities.forEach((act) => {
      act.mesh.scale.lerp(
        new THREE.Vector3(act.targetScale, act.targetScale, act.targetScale),
        0.05
      );
    });

    // Orbit camera
    if (this.camera && this.activities.length > 0) {
      this.orbitAngle += 0.002;
      const radius = 1.5 + this.clusterRadius * 1.3;
      this.camera.position.x = Math.cos(this.orbitAngle) * radius;
      this.camera.position.z = Math.sin(this.orbitAngle) * radius;
      this.camera.position.y = radius / 2;
      this.camera.lookAt(this.cameraTarget);
    }

    // Render
    if (this.renderer && this.scene && this.camera) {
      this.renderer.render(this.scene, this.camera);
    }
  },

  resetScene: function () {
    // Remove all children
    if (this.scene) {
      while (this.scene.children.length > 0) {
        this.scene.remove(this.scene.children[0]);
      }
    }

    // Reset state
    this.activities = [];
    this.clusterRadius = 4;
    this.orbitAngle = 0;
    this.basePosition = new THREE.Vector3(0, 0, 0);

    // Re-add lights
    const sunlight = new THREE.DirectionalLight(0xffffff, 1.2);
    sunlight.position.set(10, 20, 10);
    this.scene.add(sunlight);

    const ambient = new THREE.AmbientLight(0x404040);
    this.scene.add(ambient);

    // Reset camera
    if (this.camera) {
      this.camera.position.set(0, 0, 0);
      this.camera.lookAt(this.cameraTarget);
    }
  },
};
