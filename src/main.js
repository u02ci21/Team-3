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

    let selectedOutline = null; 

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

    const centerX = width / 2;
    const y = height / 2;

    const spacing = 200;
    const totalWidth = (characters.length - 1) * spacing;
    const startX = centerX - totalWidth / 2;

    characters.forEach((ch, i) => {
      const x = startX + i * spacing;

      const sprite = this.add.image(x, y, ch.key)
        .setScale(0.4)
        .setInteractive({ useHandCursor: true });
        
        const TARGET_WIDTH = 180;
        const TARGET_HEIGHT = 220;
        const scaleX = TARGET_WIDTH / sprite.width;
        const scaleY = TARGET_HEIGHT / sprite.height;
        const finalScale = Math.min(scaleX, scaleY);

        sprite.setScale(finalScale);

      this.add.text(x, y + 150, ch.name, {
        fontSize: '22px',
        color: '#fff'
      }).setOrigin(0.5);

      sprite.on('pointerover', () => sprite.setScale(finalScale * 1.1));

      sprite.on('pointerout', () => sprite.setScale(finalScale));

      sprite.on('pointerdown', () => {

        if (selectedOutline) selectedOutline.destroy();

        selectedOutline = this.add.rectangle(
          sprite.x,
          sprite.y,
          sprite.displayWidth + 12,
          sprite.displayHeight + 12
        )
          .setStrokeStyle(4, 0x00ff99)
          .setOrigin(0.5);

        console.log('CharacterSelect: Selected character =', ch.name);

        this.time.delayedCall(300, () => {
          console.log('CharacterSelect: Starting Level1 with character =', ch.name);
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
    console.log('Level1 init: Received data =', data);
    this.character = data.character;
    console.log('Level1 init: this.character =', this.character);
  }

  create() {
    console.log('Level1 create: this.character =', this.character);

    this.add.text(this.scale.width / 2, this.scale.height / 2 - 20, `Welcome, ${this.character}!`, {
      fontSize: '36px',
      color: '#fff'
    }).setOrigin(0.5);

    this.add.text(this.scale.width / 2, this.scale.height / 2 + 30, 'Level 1 Begins...', {
      fontSize: '20px',
      color: '#fff'
    }).setOrigin(0.5);

    this.time.delayedCall(2000, () => {
      console.log('Level1: Starting HarmonyGarden with character =', this.character);
      this.scene.start('HarmonyGarden', { character: this.character });
    });
  }
}

// ---------------- HARMONY GARDEN SCENE ----------------

class HarmonyGarden extends Phaser.Scene {
  constructor() {
    super('HarmonyGarden');
    this.currentStep = 0;
    this.gameStarted = false;
    this.instructions = [
      "Welcome to Harmony's Garden! I'll guide you through this journey.",
      "Your goal is to collect all the glowing flowers scattered across the garden.",
      "Use arrow keys to move your character around the map.",
      "Avoid the dark patches - they'll slow you down!",
      "Collect all flowers within the time limit to complete the level.",
      "Ready? Let's begin your adventure!"
    ];
  }

  init(data) {
    this.selectedCharacter = data.character || 'Calm';
    console.log('HarmonyGarden: Received character data:', data);
    console.log('HarmonyGarden: selectedCharacter set to:', this.selectedCharacter);
  }

  preload() {
    // Character images
    this.load.image('calm', '/assets/calm.jpg');
    this.load.image('harmony', '/assets/harmony.png');
    this.load.image('opti', '/assets/opti.jpg');
    this.load.image('hope', '/assets/hope.png');

    // Map background 
    this.load.image('gardenMap', '/assets/garden-map.png');
    
    // Bird tweeting sound
    this.load.audio('birds', '/assets/birds-tweeting.wav');
  }

  create() {
    const width = this.scale.width;
    const height = this.scale.height;

    // Add map background
    const map = this.add.image(width / 2, height / 2, 'gardenMap');
    map.setDisplaySize(width, height);

    // Play bird tweeting sound (looped)
    this.birdSound = this.sound.add('birds', { loop: true, volume: 0.3 });
    this.birdSound.play();

    // Character on the left (user's selected character)
    const characterKey = this.selectedCharacter.toLowerCase();
     console.log('Selected character:', this.selectedCharacter, 'Using key:', characterKey);
    this.playerChar = this.add.image(200, height / 2, characterKey);
    
    

    // Set consistent size for player character (smaller to not cover half screen)
    const charTargetHeight = 200;
    const charScale = charTargetHeight / this.playerChar.height;
    this.playerChar.setScale(charScale);

    // Speech bubble for player character
    this.playerBubble = this.add.rectangle(200, height / 2 - 130, 200, 80, 0xffffff, 0.9);
    this.playerBubble.setStrokeStyle(3, 0x333333);
    
    this.playerText = this.add.text(200, height / 2 - 130, '', {
      fontSize: '16px',
      color: '#333333',
      align: 'center',
      wordWrap: { width: 180 }
    }).setOrigin(0.5);

    // Harmony character on the right - same size as player
    this.harmony = this.add.image(width - 200, height / 2, 'harmony');
    const harmonyScale = charTargetHeight / this.harmony.height;
    this.harmony.setScale(harmonyScale);

    // Dialog box for Harmony (styled like the image)
    this.dialogBox = this.add.rectangle(width - 200, height / 2 - 150, 280, 120, 0xfff8dc, 0.95);
    this.dialogBox.setStrokeStyle(4, 0x8b4513);
    this.dialogBox.setOrigin(0.5);

    this.instructionText = this.add.text(width - 200, height / 2 - 150, this.instructions[0], {
      fontSize: '16px',
      color: '#333333',
      align: 'center',
      wordWrap: { width: 250 }
    }).setOrigin(0.5);

    // Back button
    this.backBtn = this.add.rectangle(width - 200, height / 2 + 50, 100, 40, 0xff6347, 0.9);
    this.backBtn.setStrokeStyle(2, 0x8b0000);
    this.backBtn.setInteractive({ useHandCursor: true });
    
    this.backText = this.add.text(width - 200, height / 2 + 50, 'Back', {
      fontSize: '18px',
      color: '#ffffff'
    }).setOrigin(0.5);

    this.backBtn.on('pointerover', () => this.backBtn.setFillStyle(0xff4500, 0.9));
    this.backBtn.on('pointerout', () => this.backBtn.setFillStyle(0xff6347, 0.9));
    this.backBtn.on('pointerdown', () => {
      if (this.currentStep > 0) {
        this.currentStep--;
        this.updateInstructions();
      }
    });

    // Next button (or "Let's Begin!")
    this.nextBtn = this.add.rectangle(width - 200, height / 2 + 100, 150, 40, 0x32cd32, 0.9);
    this.nextBtn.setStrokeStyle(2, 0x228b22);
    this.nextBtn.setInteractive({ useHandCursor: true });
    
    this.nextText = this.add.text(width - 200, height / 2 + 100, 'Next', {
      fontSize: '18px',
      color: '#ffffff'
    }).setOrigin(0.5);

    this.nextBtn.on('pointerover', () => this.nextBtn.setFillStyle(0x00ff00, 0.9));
    this.nextBtn.on('pointerout', () => this.nextBtn.setFillStyle(0x32cd32, 0.9));
    this.nextBtn.on('pointerdown', () => {
      if (this.currentStep < this.instructions.length - 1) {
        this.currentStep++;
        this.updateInstructions();
      } else {
        // Move Harmony off screen and start game
        this.startGame();
      }
    });

    // Create "Instructions" button (hidden initially, shown after game starts)
    this.instructionsBtn = this.add.rectangle(width - 100, 40, 120, 35, 0x4169e1, 0.9);
    this.instructionsBtn.setStrokeStyle(2, 0x000080);
    this.instructionsBtn.setInteractive({ useHandCursor: true });
    this.instructionsBtn.setVisible(false);
    
    this.instructionsBtnText = this.add.text(width - 100, 40, 'Instructions', {
      fontSize: '14px',
      color: '#ffffff'
    }).setOrigin(0.5);
    this.instructionsBtnText.setVisible(false);

    this.instructionsBtn.on('pointerover', () => this.instructionsBtn.setFillStyle(0x1e90ff, 0.9));
    this.instructionsBtn.on('pointerout', () => this.instructionsBtn.setFillStyle(0x4169e1, 0.9));
    this.instructionsBtn.on('pointerdown', () => {
      this.showInstructions();
    });

    // Update player responses based on step
    this.updateInstructions();
  }

  updateInstructions() {
    this.instructionText.setText(this.instructions[this.currentStep]);

    // Player character responses
    const playerResponses = [
      "OK!",
      "I get it!",
      "Got it!",
      "Understood!",
      "Ready!",
      "Let's begin!"
    ];

    this.playerText.setText(playerResponses[this.currentStep]);

    // Change button text on last step
    if (this.currentStep === this.instructions.length - 1) {
      this.nextText.setText("Let's Begin!");
    } else {
      this.nextText.setText('Next');
    }
  }

  startGame() {
    // Animate Harmony moving off screen to the right
    this.tweens.add({
      targets: [this.harmony, this.dialogBox, this.instructionText, this.backBtn, this.backText, this.nextBtn, this.nextText],
      x: this.scale.width + 300,
      duration: 800,
      ease: 'Power2',
      onComplete: () => {
        // Hide tutorial elements
        this.harmony.setVisible(false);
        this.dialogBox.setVisible(false);
        this.instructionText.setVisible(false);
        this.backBtn.setVisible(false);
        this.backText.setVisible(false);
        this.nextBtn.setVisible(false);
        this.nextText.setVisible(false);
        
        // Hide player bubble
        this.playerBubble.setVisible(false);
        this.playerText.setVisible(false);
        
        // Show instructions button
        this.instructionsBtn.setVisible(true);
        this.instructionsBtnText.setVisible(true);
        
        // Set game started flag
        this.gameStarted = true;

          // Move player character to bottom left
        this.tweens.add({
          targets: this.playerChar,
          x: 100,
          y: this.scale.height - 100,
          duration: 600,
          ease: 'Power2'
        });
        
        // TODO: Start actual gameplay here
        const gameStartedText = this.add.text(this.scale.width / 2, this.scale.height / 2, 'Game Started!', {
          fontSize: '32px',
          color: '#ffffff',
          backgroundColor: '#000000',
          padding: { x: 20, y: 10 }
        }).setOrigin(0.5);

        this.time.delayedCall(1000, () => {
          gameStartedText.destroy();
        });
      }
    });
  }

  showInstructions() {
    // Reset to first instruction
    this.currentStep = 0;
    
    // Move Harmony and UI back on screen
    this.harmony.setVisible(true);
    this.dialogBox.setVisible(true);
    this.instructionText.setVisible(true);
    this.backBtn.setVisible(true);
    this.backText.setVisible(true);
    this.nextBtn.setVisible(true);
    this.nextText.setVisible(true);
    this.playerBubble.setVisible(true);
    this.playerText.setVisible(true);
    
    // Hide instructions button
    this.instructionsBtn.setVisible(false);
    this.instructionsBtnText.setVisible(false);
    
    // Animate Harmony sliding back in
    this.harmony.x = this.scale.width + 300;
    this.dialogBox.x = this.scale.width + 300;
    this.instructionText.x = this.scale.width + 300;
    this.backBtn.x = this.scale.width + 300;
    this.backText.x = this.scale.width + 300;
    this.nextBtn.x = this.scale.width + 300;
    this.nextText.x = this.scale.width + 300;
    
    this.tweens.add({
      targets: [this.harmony, this.dialogBox, this.instructionText, this.backBtn, this.backText, this.nextBtn, this.nextText],
      x: this.scale.width - 200,
      duration: 800,
      ease: 'Power2'
    });
    
    this.updateInstructions();
  }
}



// ---------------- GAME CONFIG ----------------

const config = {
  // type: Phaser.AUTO,
  type: Phaser.CANVAS,
  width: 900,
  height: 600,
  backgroundColor: '#000000',
  parent: "game-container",
  canvas: document.getElementById('gameCanvas'),
  scene: [CharacterSelect, Level1, HarmonyGarden]

};

new Phaser.Game(config);