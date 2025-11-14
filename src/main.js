import './style.css';
import Phaser from 'phaser';

// ---------------- CHARACTER SELECT SCENE ----------------

class CharacterSelect extends Phaser.Scene {
  constructor() {
    super('CharacterSelect');
  }

  preload() {
    console.log("Loading assets...");
    this.load.image('calm', '/assets/calm.jpg');
    this.load.image('harmony', '/assets/harmony.png');
    this.load.image('opti', '/assets/opti.jpg');
    this.load.image('hope', '/assets/hope.png');
  }

  create() {
    console.log("✔ CharacterSelect scene started");

    const width = this.scale.width;
    const height = this.scale.height;

    // Create gradient background
    const ctx = this.textures.createCanvas('gradient', width, height).context;
    const grad = ctx.createLinearGradient(0, 0, width, height);

    grad.addColorStop(0, '#007bff');
    grad.addColorStop(0.25, '#00ff99');
    grad.addColorStop(0.5, '#8a2be2');
    grad.addColorStop(0.75, '#ff69b4');
    grad.addColorStop(1, '#ffd700');

    ctx.fillStyle = grad;
    ctx.fillRect(0, 0, width, height);

    this.textures.get('gradient').refresh();
    this.add.image(0, 0, 'gradient').setOrigin(0);

    this.add.text(width / 2, 60, 'Select Your Character', {
      fontSize: '36px',
      fontFamily: 'Arial',
      color: '#fff'
    }).setOrigin(0.5);

    const characters = [
      { key: 'calm', name: 'Calm' },
      { key: 'harmony', name: 'Harmony' },
      { key: 'opti', name: 'Opti' },
      { key: 'hope', name: 'Hope' }
    ];

    const spacing = 180;
    const startX = 120;
    const y = 300;
    let selectedOutline = null;

    characters.forEach((ch, i) => {
      const x = startX + i * spacing;

      const sprite = this.add.image(x, y, ch.key)
        .setScale(0.4)
        .setInteractive({ useHandCursor: true });

      this.add.text(x, y + 150, ch.name, {
        fontSize: '22px',
        color: '#fff'
      }).setOrigin(0.5);

      sprite.on('pointerover', () => sprite.setScale(0.45));

      sprite.on('pointerout', () => sprite.setScale(0.4));

      sprite.on('pointerdown', () => {
        if (selectedOutline) selectedOutline.destroy();

        selectedOutline = this.add.rectangle(
          sprite.x,
          sprite.y,
          sprite.displayWidth + 10,
          sprite.displayHeight + 10
        )
          .setStrokeStyle(4, 0x00ff99)
          .setOrigin(0.5);

        this.time.delayedCall(400, () => {
          this.scene.start('Level1', { character: ch.name });
        });
      });
    });
  }
}

// ---------------- LEVEL 1 SCENE ----------------

class Level1 extends Phaser.Scene {
  constructor() {
    super('Level1');
  }

  init(data) {
    this.character = data.character;
  }

  create() {
    this.add.text(this.scale.width / 2, this.scale.height / 2 - 20, `Welcome, ${this.character}!`, {
      fontSize: '36px',
      color: '#fff'
    }).setOrigin(0.5);

    this.add.text(this.scale.width / 2, this.scale.height / 2 + 30, 'Level 1 Begins...', {
      fontSize: '20px',
      color: '#fff'
    }).setOrigin(0.5);
  }
}

// ---------------- GAME CONFIG ----------------

const config = {
  // type: Phaser.AUTO,
  type: Phaser.CANVAS,
  width: 900,
  height: 600,
  backgroundColor: '#000000',
  canvas: document.getElementById('gameCanvas'),
  scene: [CharacterSelect, Level1]
};

new Phaser.Game(config);