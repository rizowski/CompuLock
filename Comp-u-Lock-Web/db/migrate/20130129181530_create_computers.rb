class CreateComputers < ActiveRecord::Migration
  def change
    create_table :computers do |t|
      t.references :user
      
      t.string :name
      t.string :ip_address
      t.string :enviroment

      t.timestamps
    end
  end
end
