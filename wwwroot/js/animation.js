window.Activity3D = {
    scene: null,
    camera: null,
    renderer: null,
    activities: [],
    cameraTarget: new THREE.Vector3(),
    orbitAngle: 0,
    basePosition: new THREE.Vector3(0, 0, 0),
    clusterRadius: 4, // initial cluster radius
    growthFactor: 0.1, // how much the cluster expands per new cube

    loadThreeJS: function () {
        if (!window.THREE) {
            const script = document.createElement('script');
            script.src = "https://cdn.jsdelivr.net/npm/three@0.158.0/build/three.min.js";
            script.onload = () => console.log("Three.js loaded");
            document.head.appendChild(script);
        }
    },

    init: function (containerId) {
        if (!window.THREE) {
            console.error("Three.js not loaded yet!");
            return;
        }

        const width = 800;
        const height = 600;

        this.scene = new THREE.Scene();
        this.camera = new THREE.PerspectiveCamera(60, width / height, 0.1, 2000);

        this.renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
        this.renderer.setSize(width, height);
        document.getElementById(containerId).appendChild(this.renderer.domElement);

        // Lights
        const sunlight = new THREE.DirectionalLight(0xffffff, 1.2);
        sunlight.position.set(10, 20, 10);
        this.scene.add(sunlight);
        const ambient = new THREE.AmbientLight(0x404040);
        this.scene.add(ambient);

        this.animate();
    },

addActivity: function (size = 1) {
    // Random color
    const color = Math.floor(Math.random() * 0xffffff);

    // Random spherical-ish position around center
    const theta = Math.random() * 2 * Math.PI;
    const phi = Math.random() * Math.PI;
    const r = this.clusterRadius;

    const x = this.basePosition.x + r * Math.sin(phi) * Math.cos(theta);
    const y = this.basePosition.y + r * Math.sin(phi) * Math.sin(theta);
    const z = this.basePosition.z + r * Math.cos(phi);

    // Increase cluster radius slightly for next cube
    this.clusterRadius += this.growthFactor;

    const geometry = new THREE.BoxGeometry(size, size, size); // cube
    const material = new THREE.MeshStandardMaterial({ color });
    const cube = new THREE.Mesh(geometry, material);


    // Start small for pop-in animation
    cube.scale.set(0, 0, 0);
    cube.position.set(x, y, z);

    this.scene.add(cube);
    this.activities.push({ mesh: cube, targetScale: size });
},



    animate: function () {
        requestAnimationFrame(this.animate.bind(this));

        // Animate pop-in growth
        this.activities.forEach((act) => {
            act.mesh.scale.lerp(new THREE.Vector3(act.targetScale, act.targetScale, act.targetScale), 0.05);
        });

        // Orbit camera around center cluster
        if (this.camera && this.activities.length > 0) {
            this.orbitAngle += 0.002;
            const radius = 1.5 + this.clusterRadius * 1.3; // zoom out as cluster grows
            this.camera.position.x = Math.cos(this.orbitAngle) * radius;
            this.camera.position.z = Math.sin(this.orbitAngle) * radius;
            this.camera.position.y = radius / 2;
            this.camera.lookAt(this.cameraTarget);
        }

        if (this.renderer && this.scene && this.camera) {
            this.renderer.render(this.scene, this.camera);
        }
    }
};
