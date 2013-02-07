class CreateComputers < ActiveRecord::Migration
  def change
    create_table :computers do |t|
      t.references :user
      
      t.string :name, :null => false, :default => ""
      t.string :ip_address, :null => false, :default => ""
      t.string :enviroment, :null => false, :default => ""

      t.timestamps
    end
  end
end
